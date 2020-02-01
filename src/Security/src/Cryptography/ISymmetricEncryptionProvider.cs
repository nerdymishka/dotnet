using System;
using System.IO;

namespace NerdyMishka.Security.Cryptography
{
    public interface ISymmetricEncryptionProvider : IDisposable
    {
        ReadOnlySpan<byte> Encrypt(
            ReadOnlySpan<byte> data,
            ReadOnlySpan<byte> privateKey = default,
            ReadOnlySpan<byte> symmetricKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null);

        void Encrypt(
            Stream readStream,
            Stream writeStream,
            ReadOnlySpan<byte> privateKey = default,
            ReadOnlySpan<byte> symmetricKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null);

        ReadOnlySpan<byte> Decrypt(
            ReadOnlySpan<byte> data,
            ReadOnlySpan<byte> privateKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null);

        void Decrypt(
            Stream readStream,
            Stream writeStream,
            ReadOnlySpan<byte> privateKey = default,
            ReadOnlySpan<byte> symmetricKey = default,
            IEncryptionProvider symmetricKeyEncryptionProvider = null);
    }
}