using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using NerdyMishka.Text;
using NerdyMishka.Util.Arrays;
using NerdyMishka.Util.Streams;

namespace NerdyMishka.Security.Cryptography
{
    /// <summary>
    /// A configurable symmetric encryption engine that defaults to encrypt and then MAC using
    /// AES with HMACSHA256. The primary use case is for storing data on disk and providing a
    /// cross platform alternative to DPAPI and for sending messages.
    /// </summary>
    public partial class SymmetricEncryptionProvider : IDisposable
    {
        private SymmetricAlgorithm algorithm;

        private KeyedHashAlgorithm signingAlgorithm;

        private ISymmetricEncryptionProviderOptions options;

        private bool isDisposed = false;

        public SymmetricEncryptionProvider(ISymmetricEncryptionProviderOptions options = null)
        {
            this.options = options ?? new SymmetricEncryptionProviderOptions();
        }

        /// <summary>
        /// Decrypts encrypted data and returns the decrypted bytes.
        /// </summary>
        /// <param name="blob">The data to encrypt.</param>
        /// <param name="privateKey">
        /// A password or phrase used to generate the key for the symmetric alogrithm. If the symetric
        /// key is stored with the message, the key for the symmetric algorithm is used instead.
        /// </param>
        /// <param name="symmetricKeyEncryptionProvider">
        ///  The encryption provider used to decrypt the symmetric key when it is
        ///  stored with the message.
        /// </param>
        /// <returns>Encrypted bytes.</returns>
        public ReadOnlySpan<byte> Decrypt(
            ReadOnlySpan<byte> blob,
            ReadOnlySpan<byte> privateKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null)
        {
            var pool = ArrayPool<byte>.Shared;
            byte[] rental = null;
            byte[] signerKeyRental = null;
            byte[] symmetricKeyRental = null;
            byte[] ivRental = null;

            try
            {
                rental = pool.Rent(blob.Length);
                blob.CopyTo(rental);
                using (var reader = new MemoryStream(rental))
                using (var header = this.ReadHeader(reader, this.options, privateKey, symmetricKeyEncryptionProvider))
                {
                    this.algorithm = this.algorithm ?? CreateSymmetricAlgorithm(this.options);
                    var messageSize = blob.Length - header.HeaderSize;
                    var message = new byte[messageSize];
                    Array.Copy(rental, header.HeaderSize, message, 0, messageSize);

                    if (header.Hash != null)
                    {
                        signerKeyRental = pool.Rent(header.SigningKey.Memory.Length);
                        using (var signer = this.signingAlgorithm ?? CreateSigningAlgorithm(this.options))
                        {
                            header.SigningKey.Memory.CopyTo(signerKeyRental);
                            signer.Key = signerKeyRental;
                            var h1 = header.Hash;
                            var h2 = signer.ComputeHash(message);

                            if (!h1.Memory.Span.SlowEquals(h2))
                                return null;
                        }
                    }

                    symmetricKeyRental = ArrayPool<byte>.Shared.Rent(header.SymmetricKey.Memory.Length);
                    ivRental = ArrayPool<byte>.Shared.Rent(header.IvSize);
                    header.SymmetricKey.Memory.CopyTo(symmetricKeyRental);
                    header.IV.Memory.CopyTo(ivRental);
                    using (var decryptor = this.algorithm.CreateDecryptor(symmetricKeyRental, ivRental))
                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    using (var writer = new BinaryWriter(cs))
                    {
                        writer.Write(message);
                        cs.FlushFinalBlock();
                        ms.Flush();

                        return ms.ToArray();
                    }
                }
            }
            finally
            {
                if (rental != null)
                    pool.Return(rental, true);

                if (symmetricKeyRental != null)
                    pool.Return(symmetricKeyRental, true);

                if (ivRental != null)
                    pool.Return(ivRental, true);

                if (signerKeyRental != null)
                    pool.Return(signerKeyRental, true);
            }
        }

        /// <summary>
        /// Decrypts encrypted data and returns the decrypted bytes.
        /// </summary>
        /// <param name="reader">The data stream to read from.</param>
        /// <param name="writer">The data stream to write to.</param>
        /// <param name="privateKey">
        /// A password or phrase used to generate the key for the symmetric alogrithm. If the symetric
        /// key is stored with the message, the key for the symmetric algorithm is used instead.
        /// </param>
        /// <param name="symmetricKeyEncryptionProvider">
        ///  The encryption provider used to decrypt the symmetric key when it is
        ///  stored with the message.
        /// </param>
        public void Decrypt(
            Stream reader,
            Stream writer,
            ReadOnlySpan<byte> privateKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null)
        {
            Check.NotNull(nameof(reader), reader);
            Check.NotNull(nameof(writer), writer);

            var pool = ArrayPool<byte>.Shared;
            byte[] rental = null;
            byte[] signerKeyRental = null;
            byte[] symmetricKeyRental = null;
            byte[] ivRental = null;
            byte[] buffer = null;
            try
            {
                buffer = pool.Rent(4096);
                using (var header = this.ReadHeader(reader, this.options, privateKey, symmetricKeyEncryptionProvider))
                {
                    long position = reader.Position;
                    this.algorithm = this.algorithm ?? CreateSymmetricAlgorithm(this.options);

                    if (header.Hash != null)
                    {
                        signerKeyRental = pool.Rent(header.SigningKey.Memory.Length);
                        using (var signer = CreateSigningAlgorithm(this.options))
                        {
                            header.SigningKey.Memory.CopyTo(signerKeyRental);
                            signer.Key = signerKeyRental;
                            var h1 = header.Hash;

                            long bytesRead = reader.Length - reader.Position + 1;
                            var hash = new byte[signer.OutputBlockSize];
                            while (bytesRead > 0)
                            {
                                int read = reader.Read(buffer, 0, buffer.Length);
                                bytesRead -= read;
                                signer.TransformBlock(buffer, 0, read, buffer, 0);
                            }

                            signer.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                            var h2 = signer.Hash;

                            if (!h1.Memory.Span.SlowEquals(h2))
                                return;
                        }
                    }

                    reader.Seek(position, SeekOrigin.Begin);
                    symmetricKeyRental = pool.Rent(header.SymmetricKey.Memory.Length);
                    ivRental = pool.Rent(header.IV.Memory.Length);
                    header.SymmetricKey.Memory.CopyTo(symmetricKeyRental);
                    header.IV.Memory.CopyTo(ivRental);
                    using (var decryptor = this.algorithm.CreateDecryptor(symmetricKeyRental, ivRental))
                    using (var cs = new CryptoStream(writer, decryptor, CryptoStreamMode.Write))
                    using (var bw = new BinaryWriter(cs))
                    {
                        long bytesRead = reader.Length - (reader.Position - 1);

                        while (bytesRead > 0)
                        {
                            bytesRead -= reader.Read(buffer, 0, buffer.Length);
                            bw.Write(buffer);
                        }

                        cs.FlushFinalBlock();
                        writer.Flush();
                    }
                }
            }
            finally
            {
                if (buffer != null)
                    pool.Return(buffer, true);

                if (rental != null)
                    pool.Return(rental, true);

                if (symmetricKeyRental != null)
                    pool.Return(symmetricKeyRental, true);

                if (ivRental != null)
                    pool.Return(ivRental, true);

                if (signerKeyRental != null)
                    pool.Return(signerKeyRental, true);
            }
        }

        /// <summary>
        /// Encrypts the data and returns the encrypted bytes.
        /// </summary>
        /// <param name="blob">The data to encrypt.</param>
        /// <param name="privateKey">
        ///  A password or phrase used to generate the key for the symmetric algorithm.
        /// </param>
        /// <param name="symmetricKey">
        ///  The key for the symmetric algorithm. If used, the private key is ignored
        ///  and the symetric key is stored with the message.
        /// </param>
        /// <param name="symmetricKeyEncryptionProvider">
        ///  The encryption provider used to encrypt/decrypt the symmetric key when it is
        ///  stored with the message.
        /// </param>
        /// <returns>Encrypted bytes.</returns>
        public ReadOnlySpan<byte> Encrypt(
            ReadOnlySpan<byte> blob,
            ReadOnlySpan<byte> privateKey = default,
            ReadOnlySpan<byte> symmetricKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null)
        {
            if (blob == null)
                throw new ArgumentNullException(nameof(blob));

            byte[] symmetricKeyRental = null;
            byte[] ivRental = null;
            byte[] headerRental = null;
            byte[] signingKeyRental = null;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            try
            {
                using (var header = this.GenerateHeader(this.options, symmetricKey, privateKey, null, symmetricKeyEncryptionProvider))
                {
                    byte[] encryptedBlob = null;
                    symmetricKeyRental = pool.Rent(header.SymmetricKey.Memory.Length);
                    ivRental = pool.Rent(header.IvSize);
                    header.SymmetricKey.Memory.CopyTo(symmetricKeyRental);
                    header.IV.Memory.CopyTo(ivRental);

                    this.algorithm = this.algorithm ?? CreateSymmetricAlgorithm(this.options);
                    using (var encryptor = this.algorithm.CreateEncryptor(symmetricKeyRental, ivRental))
                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new BinaryWriter(cs))
                    {
                        writer.Write(blob);
                        writer.Flush();
                        cs.Flush();
                        cs.FlushFinalBlock();
                        ms.Flush();
                        encryptedBlob = ms.ToArray();
                    }

                    headerRental = pool.Rent(header.HeaderSize);
                    header.Bytes.Memory.CopyTo(headerRental);

                    if (!this.options.SkipSigning && header.SigningKey != null && !header.SigningKey.Memory.IsEmpty)
                    {
                        signingKeyRental = pool.Rent(header.SigningKey.Memory.Length);
                        this.signingAlgorithm = this.signingAlgorithm ?? CreateSigningAlgorithm(this.options);

                        header.SigningKey.Memory.CopyTo(signingKeyRental);
                        this.signingAlgorithm.Key = signingKeyRental;
                        var hash = this.signingAlgorithm.ComputeHash(encryptedBlob);

                        Array.Copy(hash, 0, headerRental, header.Position, hash.Length);

                        hash.Clear();
                        hash = null;
                    }

                    using (var ms = new MemoryStream())
                    using (var writer = new BinaryWriter(ms))
                    {
                        writer.Write(headerRental, 0, header.HeaderSize);
                        writer.Write(encryptedBlob);
                        encryptedBlob.Clear();
                        writer.Flush();
                        ms.Flush();
                        return ms.ToArray();
                    }
                }
            }
            finally
            {
                if (symmetricKeyRental != null)
                    pool.Return(symmetricKeyRental, true);

                if (ivRental != null)
                    pool.Return(ivRental, true);

                if (headerRental != null)
                    pool.Return(headerRental, true);

                if (signingKeyRental != null)
                    pool.Return(signingKeyRental, true);
            }
        }

        /// <summary>
        /// Encrypts the data and returns the encrypted bytes.
        /// </summary>
        /// <param name="reader">The data stream to read from.</param>
        /// <param name="writer">The data stream to write to.</param>
        /// <param name="privateKey">
        ///  A password or phrase used to generate the key for the symmetric algorithm.
        /// </param>
        /// <param name="symmetricKey">
        ///  The key for the symmetric algorithm. If used, the private key is ignored
        ///  and the symetric key is stored with the message.
        /// </param>
        /// <param name="symmetricKeyEncryptionProvider">
        ///  The encryption provider used to encrypt/decrypt the symmetric key when it is
        ///  stored with the message.
        /// </param>
        public void Encrypt(
            Stream reader,
            Stream writer,
            ReadOnlySpan<byte> privateKey = default,
            ReadOnlySpan<byte> symmetricKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null)
        {
            Check.NotNull(nameof(reader), reader);
            Check.NotNull(nameof(writer), writer);

            byte[] symmetricKeyRental = null;
            byte[] ivRental = null;
            byte[] headerRental = null;
            byte[] signingKeyRental = null;
            byte[] buffer = null;
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            try
            {
                buffer = pool.Rent(4096);
                using (var header = this.GenerateHeader(this.options, symmetricKey, privateKey, null, symmetricKeyEncryptionProvider))
                {
                    symmetricKeyRental = pool.Rent(header.SymmetricKey.Memory.Length);
                    ivRental = pool.Rent(header.IvSize);
                    header.SymmetricKey.Memory.CopyTo(symmetricKeyRental);
                    header.IV.Memory.CopyTo(ivRental);
                    this.algorithm = this.algorithm ?? CreateSymmetricAlgorithm(this.options);

                    using (var encryptor = this.algorithm.CreateEncryptor(symmetricKeyRental, ivRental))
                    using (var cs = new CryptoStream(writer, encryptor, CryptoStreamMode.Write))
                    using (var bw = new BinaryWriter(cs, Utf8.NoBom, false))
                    {
                        bw.Write(header.Bytes.Memory);
                        long bytesLeft = reader.Length;
                        while (bytesLeft > 0)
                        {
                            bytesLeft -= reader.Read(buffer, 0, buffer.Length);
                            bw.Write(buffer);
                        }

                        cs.Flush();
                        cs.FlushFinalBlock();
                        writer.Flush();
                    }

                    headerRental = pool.Rent(header.Bytes.Memory.Length);
                    header.Bytes.Memory.CopyTo(headerRental);

                    if (!this.options.SkipSigning && header.SigningKey != null && !header.SigningKey.Memory.IsEmpty)
                    {
                        signingKeyRental = pool.Rent(header.SigningKey.Memory.Length);
                        using (var signer = CreateSigningAlgorithm(this.options))
                        {
                            signer.Key = signingKeyRental;
                            writer.Position = header.Bytes.Memory.Length;

                            writer.Seek(header.Bytes.Memory.Length - 1, SeekOrigin.Begin);
                            long bytesRead = writer.Length - (writer.Position + 1);
                            while (bytesRead > 0)
                            {
                                int read = writer.Read(buffer, 0, buffer.Length);
                                bytesRead -= read;
                                signer.TransformBlock(buffer, 0, read, buffer, 0);
                            }

                            signer.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                            var hash = signer.Hash;

                            Array.Copy(hash, 0, headerRental, header.Position, hash.Length);
                            hash.Clear();
                        }
                    }

                    writer.Seek(0, SeekOrigin.Begin);
                    using (var bw = new BinaryWriter(writer, Utf8.NoBom, true))
                    {
                        bw.Write(headerRental);
                        writer.Flush();
                        writer.Seek(0, SeekOrigin.End);
                    }
                }
            }
            finally
            {
                if (buffer != null)
                    pool.Return(buffer, true);

                if (symmetricKeyRental != null)
                    pool.Return(symmetricKeyRental, true);

                if (ivRental != null)
                    pool.Return(ivRental, true);

                if (headerRental != null)
                    pool.Return(headerRental, true);

                if (signingKeyRental != null)
                    pool.Return(signingKeyRental, true);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;

            if (disposing)
            {
                this.algorithm?.Dispose();
                this.signingAlgorithm?.Dispose();
                this.options?.Dispose();
            }

            this.isDisposed = true;
        }
    }
}