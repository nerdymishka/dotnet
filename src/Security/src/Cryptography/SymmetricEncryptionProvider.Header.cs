using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using NerdyMishka.Text;
using NerdyMishka.Util.Arrays;
using NerdyMishka.Util.Streams;

namespace NerdyMishka.Security.Cryptography
{
    /// <summary>
    /// PArtial class.
    /// </summary>
    public partial class SymmetricEncryptionProvider
    {
        protected internal Header ReadHeader(
            Stream reader,
            ISymmetricEncryptionProviderOptions options,
            ReadOnlySpan<byte> privateKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null)
        {
            Check.NotNull(nameof(reader), reader);
            Check.NotNull(nameof(options), options);

            ReadOnlySpan<byte> signingKey = default;

            if (options.SigningKey != null)
                signingKey = options.SigningKey.Memory.Span;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, Utf8.NoBom, true))
            using (var br = new BinaryReader(reader, Utf8.NoBom, true))
            {
                var version = br.ReadInt16();
                bw.Write(version);
                Header header = null;
                switch (version)
                {
                    case 1:
                    default:
                        header = new HeaderV1();
                        break;
                }

                // header shorts/ints
                // 1. version
                // 2. algo
                // 3. signing,
                // 2. metadataSize
                // 3. symmetricSaltSize
                // 4. signingSaltSize
                // 5. ivSize
                // 6. symmetricKeySize
                // 7. hashSize

                // header values
                // 1. metadata
                // 2. symmetricSalt
                // 3. signingSalt
                // 4. iv
                // 5. symmetricKey
                // 6. hash
                header.SymmetricAlgorithmType = (SymmetricAlgorithmType)br.ReadInt16();
                header.KeyedHashAlgorithmType = (KeyedHashAlgorithmType)br.ReadInt16();
                header.MetaDataSize = br.ReadInt32();
                header.Iterations = br.ReadInt32();
                header.SymmetricSaltSize = br.ReadInt16();
                header.SigningSaltSize = br.ReadInt16();
                header.IvSize = br.ReadInt16();
                header.SymmetricKeySize = br.ReadInt16();
                header.HashSize = br.ReadInt16();

                bw.Write((short)header.SymmetricAlgorithmType);
                bw.Write((short)header.KeyedHashAlgorithmType);
                bw.Write(header.MetaDataSize);
                bw.Write(header.Iterations);
                bw.Write(header.SymmetricSaltSize);
                bw.Write(header.SigningSaltSize);
                bw.Write(header.IvSize);
                bw.Write(header.SymmetricKeySize);
                bw.Write(header.HashSize);

                if (options.SymmetricAlgorithm != header.SymmetricAlgorithmType)
                {
                    options.SymmetricAlgorithm = header.SymmetricAlgorithmType;
                    this.algorithm = null;
                }

                if (options.KeyedHashedAlgorithm != header.KeyedHashAlgorithmType)
                {
                    options.KeyedHashedAlgorithm = header.KeyedHashAlgorithmType;
                    this.signingAlgorithm = null;
                }

                byte[] metadata = null;
                byte[] symmetricSalt = null;
                ReadOnlySpan<byte> signingSalt = default;
                byte[] iv = null;
                ReadOnlySpan<byte> symmetricKey = default;
                byte[] hash = null;

                if (header.MetaDataSize > 0)
                {
                    metadata = br.ReadBytes(header.MetaDataSize);
                    bw.Write(metadata);
                }

                if (header.SymmetricSaltSize > 0)
                {
                    symmetricSalt = br.ReadBytes(header.SymmetricSaltSize);
                    bw.Write(symmetricSalt);
                }

                if (header.SigningSaltSize > 0)
                {
                    signingSalt = br.ReadBytes(header.SigningSaltSize);
                    bw.Write(signingSalt);
                }

                if (header.IvSize > 0)
                {
                    iv = br.ReadBytes(header.IvSize);
                    bw.Write(iv);
                }

                if (header.SymmetricKeySize > 0)
                {
                    symmetricKey = br.ReadBytes(header.SymmetricKeySize);
                    bw.Write(symmetricKey);
                }

                if (header.HashSize > 0)
                {
                    hash = br.ReadBytes(header.HashSize);
                    bw.Write(hash);
                }

                bw.Flush();
                ms.Flush();
                {
                    var bytes = ms.ToArray();
                    var buffer = MemoryPool<byte>.Shared.Rent(bytes.Length);
                    bytes.CopyTo(buffer.Memory.Span);
                    header.Bytes = buffer;
                }

                header.Position = reader.Position;

                if (symmetricKeyEncryptionProvider != null)
                    symmetricKey = symmetricKeyEncryptionProvider.Decrypt(symmetricKey);

                if (symmetricKey == null && privateKey.IsEmpty)
                    throw new ArgumentNullException(nameof(privateKey),
                        "privateKey or symmetricKey must have a value");

                if (!options.SkipSigning && privateKey == null && signingKey.IsEmpty)
                    throw new ArgumentNullException(nameof(privateKey),
                        "privateKey must have a value or options.SigningKey must have a value or options.SkipSigning must be true");

                if (symmetricKey == null)
                {
                    if (symmetricSalt == null)
                        throw new InvalidOperationException("symmetricSalt for the privateKey could not be retrieved");

                    using (var generator = new Rfc2898DeriveBytes(privateKey, symmetricSalt, header.Iterations, HashAlgorithmName.SHA256))
                    {
                        var buffer = MemoryPool<byte>.Shared.Rent(options.KeySize / 8);
                        ReadOnlySpan<byte> bytes = generator.GetBytes(options.KeySize / 8);
                        bytes.CopyTo(buffer.Memory.Span);
                        header.SymmetricKey = buffer;
                    }
                }

                if (!options.SkipSigning && signingKey.IsEmpty)
                {
                    if (signingSalt == null)
                        throw new InvalidOperationException("symmetricSalt for the privateKey could not be retrieved");

                    var key = !symmetricKey.IsEmpty ? symmetricKey : privateKey;
                    using (var generator = new Rfc2898DeriveBytes(key, signingSalt, header.Iterations, HashAlgorithmName.SHA256))
                    {
                        generator.IterationCount = header.Iterations;
                        var buffer = MemoryPool<byte>.Shared.Rent(options.KeySize / 8);
                        ReadOnlySpan<byte> bytes = generator.GetBytes(options.KeySize / 8);
                        bytes.CopyTo(buffer.Memory.Span);
                        header.SigningKey = buffer;
                    }
                }

                {
                    var buffer = MemoryPool<byte>.Shared.Rent(options.KeySize / 8);
                    symmetricKey.CopyTo(buffer.Memory.Span);
                    header.SymmetricKey = buffer;

                    buffer = MemoryPool<byte>.Shared.Rent(iv.Length);
                    iv.CopyTo(buffer.Memory.Span);
                    header.IV = buffer;

                    buffer = MemoryPool<byte>.Shared.Rent(hash.Length);
                    hash.CopyTo(buffer.Memory.Span);
                    header.Hash = buffer;
                }

                return header;
            }
        }

        protected internal Header GenerateHeader(
            ISymmetricEncryptionProviderOptions options,
            ReadOnlySpan<byte> symmetricKey = default,
            ReadOnlySpan<byte> privateKey = default,
            ReadOnlySpan<byte> metadata = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null)
        {
            Check.NotNull(nameof(options), options);
            privateKey = !privateKey.IsEmpty ? privateKey : options.Key.Memory.Span;

            ReadOnlySpan<byte> signingKey = default;
            if (options.SigningKey != null)
                signingKey = options.SigningKey.Memory.Span;

            // header values
            // 1. version
            // 2. metadataSize
            // 3. iterations
            // 4. symmetricSaltSize
            // 5. signingSaltSize
            // 6. ivSize
            // 7. symmetricKeySize
            // 8. hashSize

            // header values
            // 1. metadata
            // 2. symmetricSalt
            // 3. signingSalt
            // 4. iv
            // 5. symmetricKey
            // 6. hash
            var header = new HeaderV1();
            header.MetaDataSize = metadata.Length;

            bool privateKeyEmpty = privateKey == null || privateKey.IsEmpty;
            bool symmetricKeyEmpty = symmetricKey == null || symmetricKey.IsEmpty;

            if (privateKeyEmpty && symmetricKeyEmpty)
                throw new ArgumentNullException(nameof(privateKey), "privateKey or symmetricKey must have a value");

            if (!options.SkipSigning && privateKeyEmpty && signingKey.IsEmpty)
                throw new ArgumentNullException(nameof(privateKey),
                    "privateKey must have a value or options.SigningKey must have a value or options.SkipSigning must be true");

            if (!privateKeyEmpty)
            {
                header.SymmetricSaltSize = (short)(options.SaltSize / 8);

                if (!options.SkipSigning && signingKey.IsEmpty)
                {
                    header.SigningSaltSize = (short)(options.SaltSize / 8);
                    this.signingAlgorithm = this.signingAlgorithm ?? CreateSigningAlgorithm(options);
                }
            }

            if (!symmetricKeyEmpty)
            {
                header.SymmetricKeySize = (short)(options.KeySize / 8);
            }

            this.algorithm = this.algorithm ?? CreateSymmetricAlgorithm(options);
            this.algorithm.GenerateIV();
            var iv = this.algorithm.IV;
            header.IvSize = (short)iv.Length;
            {
                var buffer = MemoryPool<byte>.Shared.Rent(iv.Length);
                iv.CopyTo(buffer.Memory.Span);
                header.IV = buffer;
            }

            header.HashSize = (short)(this.signingAlgorithm.HashSize / 8);
            header.Iterations = options.Iterations;
            using (var ms = new MemoryStream(new byte[header.HeaderSize]))
            using (var bw = new BinaryWriter(ms, Utf8.NoBom, false))
            {
                if (!symmetricKey.IsEmpty && symmetricKeyEncryptionProvider != null)
                {
                    symmetricKey = symmetricKeyEncryptionProvider.Encrypt(symmetricKey);
                    header.SymmetricKeySize = (short)symmetricKey.Length;
                }

                header.SymmetricAlgorithmType = options.SymmetricAlgorithm;
                header.KeyedHashAlgorithmType = options.KeyedHashedAlgorithm;

                bw.Write(header.Version);
                bw.Write((short)header.SymmetricAlgorithmType);
                bw.Write((short)header.KeyedHashAlgorithmType);
                bw.Write(header.MetaDataSize);
                bw.Write(header.Iterations);
                bw.Write(header.SymmetricSaltSize);
                bw.Write(header.SigningSaltSize);
                bw.Write(header.IvSize);
                bw.Write(header.SymmetricKeySize);
                bw.Write(header.HashSize);

                if (privateKey != null)
                {
                    ReadOnlySpan<byte> symmetricSalt = GenerateSalt(header.SymmetricSaltSize);

                    using (var generator = new Rfc2898DeriveBytes(privateKey, symmetricSalt, options.Iterations, HashAlgorithmName.SHA256))
                    {
                        symmetricSalt = generator.Salt;
                        ReadOnlySpan<byte> bytes = generator.GetBytes(options.KeySize / 8);
                        var buffer = MemoryPool<byte>.Shared.Rent(options.KeySize / 8);
                        bytes.CopyTo(buffer.Memory.Span);
                        header.SymmetricKey = buffer;

                        bw.Write(symmetricSalt);
                    }

                    if (!options.SkipSigning || !signingKey.IsEmpty)
                    {
                        var signingSalt = GenerateSalt(header.SigningSaltSize);

                        using (var generator = new Rfc2898DeriveBytes(
                            privateKey, signingSalt, options.Iterations, HashAlgorithmName.SHA256))
                        {
                            signingSalt = generator.Salt;
                            signingKey = generator.GetBytes(options.KeySize / 8);
                            var buffer = MemoryPool<byte>.Shared.Rent(options.KeySize / 8);
                            signingKey.CopyTo(buffer.Memory.Span);
                            header.SigningKey = buffer;
                            bw.Write(signingSalt);
                        }

                        signingSalt.Clear();
                    }
                }

                bw.Write(iv);
                if (!symmetricKeyEmpty)
                {
                    bw.Write(symmetricKey);
                }

                bw.Flush();
                ms.Flush();
                header.Position = ms.Position;

                ReadOnlySpan<byte> data = ms.ToArray();
                header.Bytes = MemoryPool<byte>.Shared.Rent(header.HeaderSize);
                data.CopyTo(header.Bytes.Memory.Span);
            }

            return header;
        }

        protected static byte[] GenerateSalt(int length)
        {
            using (var rng = new RandomNumberGenerator())
            {
                return rng.NextBytes(length);
            }
        }

        private static SymmetricAlgorithm CreateSymmetricAlgorithm(
            ISymmetricEncryptionProviderOptions options)
        {
            if (options.SymmetricAlgorithm == SymmetricAlgorithmType.None)
                throw new ArgumentException("SymmetricAlgo", nameof(options));

            var algorithm = SymmetricAlgorithm.Create(options.SymmetricAlgorithm.ToString());
            algorithm.KeySize = options.KeySize;
            algorithm.Padding = options.Padding;
            algorithm.Mode = options.Mode;
            algorithm.Padding = options.Padding;

            return algorithm;
        }

        private static KeyedHashAlgorithm CreateSigningAlgorithm(ISymmetricEncryptionProviderOptions options)
        {
            if (options.KeyedHashedAlgorithm == KeyedHashAlgorithmType.None)
                return null;

            return KeyedHashAlgorithm.Create(options.KeyedHashedAlgorithm.ToString());
        }

        protected internal class HeaderV1 : Header
        {
            // int size
            // short size.
            // meta data size.
            public override int HeaderSize =>
                (2 * 4) +
                (8 * 2) +
                this.MetaDataSize +
                this.SymmetricKeySize +
                this.SymmetricSaltSize +
                this.SigningSaltSize +
                this.IvSize +
                this.HashSize;
        }

        protected internal class Header : IDisposable
        {
            public virtual short Version { get; } = 1;

            public int MetaDataSize { get; set; }

            public short SymmetricKeySize { get; set; }

            public short SymmetricSaltSize { get; set; }

            public short SigningSaltSize { get; set; }

            public short IvSize { get; set; }

            public short HashSize { get; set; }

            public SymmetricAlgorithmType SymmetricAlgorithmType { get; set; }

            public KeyedHashAlgorithmType KeyedHashAlgorithmType { get; set; }

            public IMemoryOwner<byte> SymmetricKey { get; set; }

            public IMemoryOwner<byte> SigningKey { get; set; }

            public IMemoryOwner<byte> IV { get; set; }

            public int Iterations { get; set; }

            public long Position { get; set; } = 0;

            public IMemoryOwner<byte> Bytes { get; set; }

            public IMemoryOwner<byte> Hash { get; set; }

            public virtual int HeaderSize => 0;

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.IV?.Dispose();
                    this.Hash?.Dispose();
                    this.Bytes?.Dispose();
                    this.SymmetricKey?.Dispose();
                    this.SigningKey?.Dispose();
                }
            }
        }
    }
}