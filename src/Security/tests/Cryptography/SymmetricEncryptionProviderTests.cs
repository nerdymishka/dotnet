using System;
using System.IO;

using Mettle;
using NerdyMishka.Security.Cryptography;
using NerdyMishka.Text;

public class SymmetricEncryptionProviderTests
{
    private IAssert assert;

    public SymmetricEncryptionProviderTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Temp()
    {
        ReadOnlySpan<byte> privateKey = null;
        ReadOnlySpan<byte> salt = null;
        using (var rng = new RandomNumberGenerator())
        {
            privateKey = rng.NextBytes(20);
            salt = rng.NextBytes(64 / 8);
        }

        var name = System.Security.Cryptography.HashAlgorithmName.SHA256;
        byte[] pw1 = null;
        byte[] pw2 = null;
        using (var generator = new Rfc2898DeriveBytes(privateKey, salt, 100, name))
        {
            pw1 = generator.GetBytes(256 / 8);
        }

        using (var generator = new Rfc2898DeriveBytes(privateKey, salt, 100, name))
        {
            // assert.Equal(salt, generator.Salt);
            pw2 = generator.GetBytes(256 / 8);
        }

        assert.Equal(pw1, pw2);
    }


    [UnitTest]
    public void EncryptDecryptBlob_WithPrivateKey()
    {
        using (var options = new SymmetricEncryptionProviderOptions())
        using (var engine = new SymmetricEncryptionProvider(options))
        {
            byte[] privateKey = null;
            using (var rng = new RandomNumberGenerator())
            {
                privateKey = rng.NextBytes(20);
            }

            ReadOnlySpan<byte> text = Utf8.NoBom.GetBytes("My name Jeff");

            var encryptedBlob = engine.Encrypt(text, privateKey);
            assert.True(!encryptedBlob.IsEmpty);
            assert.True(encryptedBlob != text);

            var data = new byte[encryptedBlob.Length];
            encryptedBlob.CopyTo(data);

            var text2 = engine.Decrypt(encryptedBlob, privateKey);
            assert.True(!text2.IsEmpty);
            assert.Equal(text.Length, text2.Length);
            for (var i = 0; i < text2.Length; i++)
            {
                assert.Equal(text[i], text2[i]);
            }
        }
    }

    [UnitTest]
    public void ReaderHeader_WithPrivateKey()
    {
        using (var options = new SymmetricEncryptionProviderOptions())
        using (var engine = new SymmetricEncryptionProvider())
        {
            byte[] privateKey = null;
            using (var rng = new RandomNumberGenerator())
            {
                privateKey = rng.NextBytes(20);
            }

            byte[] data = null;
            var header1 = engine.GenerateHeader(options, privateKey: privateKey);
            data = new byte[header1.Bytes.Memory.Length];
            header1.Bytes.Memory.CopyTo(data);


            var ms = new MemoryStream(data);
            using (var header = engine.ReadHeader(ms, options, privateKey))
            {
                ms.Position = 0;

                var data2 = new byte[header.Bytes.Memory.Length];
                header.Bytes.Memory.CopyTo(data2);

                assert.Equal(data.Length, data2.Length);
                assert.Equal(data, data2);

                assert.NotNull(header);
                assert.Equal(1, header.Version);
                assert.Equal(SymmetricAlgorithmType.AES, header.SymmetricAlgorithmType);
                assert.Equal(KeyedHashAlgorithmType.HMACSHA256, header.KeyedHashAlgorithmType);
                assert.Equal(0, header.MetaDataSize);
                assert.NotEqual(0, header.SigningSaltSize);
                assert.NotEqual(0, header.SymmetricSaltSize);
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

                ms.Position = 0;
                using (var br = new BinaryReader(ms))
                {
                    assert.Equal(1, br.ReadInt16());
                    assert.Equal((short)SymmetricAlgorithmType.AES, br.ReadInt16());
                    assert.Equal((short)KeyedHashAlgorithmType.HMACSHA256, br.ReadInt16());
                    assert.Equal(header.MetaDataSize, br.ReadInt32());
                    assert.Equal(header.Iterations, br.ReadInt32());
                    assert.Equal(header.SymmetricSaltSize, br.ReadInt16());
                    assert.Equal(header.SigningSaltSize, br.ReadInt16());
                    assert.Equal(header.IvSize, br.ReadInt16());
                    assert.Equal(header.SymmetricKeySize, br.ReadInt16());
                    assert.Equal(header.HashSize, br.ReadInt16());

                    assert.Equal(header.Version, header1.Version);
                    assert.Equal(header.KeyedHashAlgorithmType, header1.KeyedHashAlgorithmType);
                    assert.Equal(header.SymmetricAlgorithmType, header1.SymmetricAlgorithmType);
                    assert.Equal(header.MetaDataSize, header1.MetaDataSize);
                    assert.Equal(header.Iterations, header1.Iterations);
                    assert.Equal(header.SymmetricSaltSize, header1.SymmetricSaltSize);
                    assert.Equal(header.SigningSaltSize, header1.SigningSaltSize);
                    assert.Equal(header.IvSize, header1.IvSize);
                    assert.Equal(header.SymmetricKeySize, header1.SymmetricKeySize);
                    assert.Equal(header.HashSize, header1.HashSize);

                    byte[] metadata = null;
                    byte[] symmetricSalt = null;
                    byte[] signingSalt = null;
                    byte[] iv = null;
                    byte[] symmetricKey = null;
                    byte[] hash = null;

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
                    // 1. metadata (optional)
                    // 2. symmetricSalt (optional)
                    // 3. signingSalt (optional)
                    // 4. iv
                    // 5. symmetricKey (optional)
                    // 6. hash

                    if (header.MetaDataSize > 0)
                        metadata = br.ReadBytes(header.MetaDataSize);

                    if (header.SymmetricSaltSize > 0)
                    {
                        assert.Equal(options.SaltSize / 8, header.SymmetricSaltSize);
                        var name = System.Security.Cryptography.HashAlgorithmName.SHA256;
                        symmetricSalt = br.ReadBytes(header.SymmetricSaltSize);
                        using (var generator = new Rfc2898DeriveBytes(privateKey, symmetricSalt, options.Iterations, name))
                        {
                            symmetricKey = generator.GetBytes(options.KeySize / 8);
                            var p1 = new byte[header.SymmetricKey.Memory.Length];
                            var p2 = new byte[header1.SymmetricKey.Memory.Length];
                            header.SymmetricKey.Memory.CopyTo(p1);
                            header1.SymmetricKey.Memory.CopyTo(p2);
                            assert.Equal(p1, p2);

                            assert.Equal(symmetricKey, p1);
                        }

                        symmetricKey = null;
                    }

                    if (header.SigningSaltSize > 0)
                    {
                        signingSalt = br.ReadBytes(header.SigningSaltSize);
                        var name = System.Security.Cryptography.HashAlgorithmName.SHA256;
                        using (var generator = new Rfc2898DeriveBytes(privateKey, signingSalt, options.Iterations, name))
                        {
                            var signingKey = generator.GetBytes(options.KeySize / 8);
                            var p1 = new byte[header.SymmetricKey.Memory.Length];
                            header.SigningKey.Memory.CopyTo(p1);

                            assert.Equal(signingKey, p1);
                        }
                    }

                    if (header.IvSize > 0)
                    {
                        iv = br.ReadBytes(header.IvSize);

                        var iv2 = new byte[header.IvSize];
                        header.IV.Memory.CopyTo(iv2);
                        assert.Equal(iv, iv2);
                    }

                    if (header.SymmetricKeySize > 0)
                        symmetricKey = br.ReadBytes(header.SymmetricKeySize);

                    if (header.HashSize > 0)
                    {
                        hash = br.ReadBytes(header.HashSize);

                        var hash2 = new byte[header.HashSize];
                        header.Hash.Memory.CopyTo(hash2);
                        assert.Equal(hash, hash2);
                    }

                    assert.Null(metadata);
                    assert.NotNull(hash);
                    assert.NotNull(signingSalt);
                    assert.NotNull(symmetricSalt);

                    // header property has a copy but does not
                    // write it to the file header when a private key
                    // is provided.
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