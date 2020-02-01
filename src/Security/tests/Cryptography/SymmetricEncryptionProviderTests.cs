using System.IO;
using Mettle;
using NerdyMishka.Security.Cryptography;

public class SymmetricEncryptionProviderTests
{
    private IAssert assert;

    public SymmetricEncryptionProviderTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void GenerateHeader_WithPrivateKey()
    {
        using (var engine = new SymmetricEncryptionProvider())
        {
            byte[] privateKey = null;
            using (var rng = new RandomNumberGenerator())
            {
                privateKey = rng.NextBytes(20);
            }

            using (var options = new SymmetricEncryptionProviderOptions())
            using (var header = engine.GenerateHeader(options, privateKey: privateKey))
            {
                assert.NotNull(header);
                assert.Equal(1, header.Version);
                assert.Equal(SymmetricAlgorithmType.AES, header.SymmetricAlgorithmType);
                assert.Equal(KeyedHashAlgorithmType.HMACSHA256, header.KeyedHashAlgorithmType);
                assert.Equal(0, header.MetaDataSize);
                assert.NotEqual(0, header.SigningSaltSize);
                assert.NotEqual(0, header.SymmetricSaltSize);
                assert.Equal(8, header.SigningSaltSize);
                assert.Equal(8, header.SymmetricSaltSize);
                assert.NotEqual(0, header.IvSize);
                assert.NotEqual(0, header.HashSize);
                assert.NotEqual(0, header.Iterations);
                assert.NotNull(header.SymmetricKey);
                assert.NotNull(header.IV);
                assert.NotNull(header.SigningKey);
                assert.NotNull(header.Bytes);

                assert.Ok(!header.SymmetricKey.Memory.IsEmpty);
                assert.Ok(!header.IV.Memory.IsEmpty);
                assert.Ok(!header.SigningKey.Memory.IsEmpty);

                var temp = new byte[header.SymmetricKey.Memory.Length];
                header.SymmetricKey.Memory.CopyTo(temp);
                assert.NotEqual(privateKey, temp);

                assert.Ok(!header.Bytes.Memory.IsEmpty);

                temp = new byte[header.Bytes.Memory.Length];
                header.Bytes.Memory.CopyTo(temp);

                using (var ms = new MemoryStream(temp))
                using (var br = new BinaryReader(ms))
                {
                    assert.Equal(header.Version, br.ReadInt16());
                    assert.Equal((short)SymmetricAlgorithmType.AES, br.ReadInt16());
                    assert.Equal((short)KeyedHashAlgorithmType.HMACSHA256, br.ReadInt16());
                    assert.Equal(header.MetaDataSize, br.ReadInt32());
                    assert.Equal(header.Iterations, br.ReadInt32());
                    assert.Equal(header.SymmetricSaltSize, br.ReadInt16());
                    assert.Equal(header.SigningSaltSize, br.ReadInt16());
                    assert.Equal(header.IvSize, br.ReadInt16());
                    assert.Equal(header.SymmetricKeySize, br.ReadInt16());
                    assert.Equal(header.HashSize, br.ReadInt16());

                    byte[] metadata = null;
                    byte[] symmetricSalt = null;
                    byte[] signingSalt = null;
                    byte[] iv = null;
                    byte[] symmetricKey = null;
                    byte[] hash = null;

                    if (header.MetaDataSize > 0)
                        metadata = br.ReadBytes(header.MetaDataSize);

                    if (header.SymmetricSaltSize > 0)
                        symmetricSalt = br.ReadBytes(header.SymmetricSaltSize);

                    if (header.SigningSaltSize > 0)
                        signingSalt = br.ReadBytes(header.SigningSaltSize);

                    if (header.IvSize > 0)
                        iv = br.ReadBytes(header.IvSize);

                    if (header.SymmetricKeySize > 0)
                        symmetricKey = br.ReadBytes(header.SymmetricKeySize);

                    if (header.HashSize > 0)
                        hash = br.ReadBytes(header.HashSize);

                    assert.Null(metadata);
                    assert.NotNull(hash);
                    assert.NotNull(signingSalt);
                    assert.NotNull(symmetricSalt);

                    // header property has a copy but does not
                    // write it to the file header when a private key
                    // is provided. The private key is external and is
                    // used to generate the symmetricKey.
                    assert.Null(symmetricKey);
                    assert.NotNull(iv);
                    assert.NotEmpty(hash);
                    assert.NotEmpty(signingSalt);
                    assert.NotEmpty(symmetricSalt);

                    assert.NotEmpty(iv);
                }
            }
        }
    }
}