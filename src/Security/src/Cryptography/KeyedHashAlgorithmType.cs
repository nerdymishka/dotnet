#pragma warning disable CA1028

namespace NerdyMishka.Security.Cryptography
{
    public enum KeyedHashAlgorithmType : short
    {
        None = 0,

        HMACMD5 = 1,

        HMACRIPEMD160 = 2,

        HMACSHA1 = 3,

        HMACSHA256 = 4,

        HMACSHA384 = 5,

        HMACSHA512 = 6,
    }
}