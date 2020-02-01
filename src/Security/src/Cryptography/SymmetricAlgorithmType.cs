#pragma warning disable CA1028

namespace NerdyMishka.Security.Cryptography
{
    public enum SymmetricAlgorithmType : short
    {
        /// <summary>No Algorithm.</summary>
        None = 0,

        /// <summary>American Encryption Standard</summary>
        AES = 1,
    }
}