using System;
using System.Buffers;
using System.Security.Cryptography;

namespace NerdyMishka.Security.Cryptography
{
    public class SymmetricEncryptionProviderOptions : ISymmetricEncryptionProviderOptions, IDisposable
    {
        private bool isDisposed = false;

        public int KeySize { get; set; } = 256;

        public int BlockSize { get; set; } = 128;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Security",
            "SCS0011:Weak Cipher",
            Justification = "EncryptionProvider pushes the consumer to encrypt and MAC the data")]
        public CipherMode Mode { get; set; } = CipherMode.CBC;

        public PaddingMode Padding { get; set; } = PaddingMode.PKCS7;

        public SymmetricAlgorithmType SymmetricAlgorithm { get; set; }
            = SymmetricAlgorithmType.AES;

        public KeyedHashAlgorithmType KeyedHashedAlgorithm { get; set; }
            = KeyedHashAlgorithmType.HMACSHA256;

        public int SaltSize { get; set; } = 64;

        public int Iterations { get; set; } = 10000;

        public int MinimumPrivateKeyLength { get; set; } = 12;

        public bool SkipSigning { get; set; } = false;

        public IMemoryOwner<byte> Key { get; set; }

        public IMemoryOwner<byte> SigningKey { get; set; }

        public void SetKey(ReadOnlySpan<byte> key)
        {
            var rental = MemoryPool<byte>.Shared.Rent(key.Length);
            key.CopyTo(rental.Memory.Span);
            this.Key = rental;
        }

        public void SetSigningKey(ReadOnlySpan<byte> signingKey)
        {
            var rental = MemoryPool<byte>.Shared.Rent(signingKey.Length);
            signingKey.CopyTo(rental.Memory.Span);
            this.SigningKey = rental;
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
                this.Key?.Dispose();
                this.SigningKey?.Dispose();
            }

            this.isDisposed = true;
        }
    }
}