using System;
using System.Security.Cryptography;

namespace NerdyMishka.Security.Cryptography
{
    public interface ISymmetricEncryptionProviderOptions
    {
        int KeySize { get; set; }

        int BlockSize { get; set; }

        CipherMode Mode { get; set; }

        PaddingMode Padding { get; set; }

        SymmetricAlgorithmType SymmetricAlgorithm { get; set; }

        KeyedHashAlgorithmType KeyedHashedAlgorithm { get; set; }

        int SaltSize { get; set; }

        int Iterations { get; set; }

        int MinimumPrivateKeyLength { get; set; }

        bool SkipSigning { get; set; }

        ReadOnlyMemory<byte> Key { get; set; }

        ReadOnlyMemory<byte> SigningKey { get; set; }
    }
}