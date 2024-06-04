using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Security
{
    // Generates a cryptographically secure random salt
    public static byte[] GenerateRandomSalt(int size = 16) // Default size is 16 bytes
    {
        byte[] salt = new byte[size];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    // Derives a key and IV from a given passphrase and salt
    private static (byte[] Key, byte[] IV) DeriveKeyAndIV(string passphrase, byte[] salt)
    {
        using (var keyGenerator = new Rfc2898DeriveBytes(passphrase, salt, 10000))
        {
            byte[] key = keyGenerator.GetBytes(16); // AES-128 bit key
            byte[] iv = keyGenerator.GetBytes(16);  // AES block size is 128 bits
            return (key, iv);
        }
    }

    // Encrypts a string and returns the encrypted data as a Base64 string
    public static string EncryptStringToString(string plainText, string passphrase)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = EncryptBytesToBytes(bytes, passphrase);
        return Convert.ToBase64String(encryptedBytes);
    }

    // Decrypts a Base64 encoded string and returns the plaintext
    public static string DecryptStringToString(string encryptedDataWithSaltBase64, string passphrase)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedDataWithSaltBase64);
        byte[] decryptedBytes = DecryptBytesToBytes(encryptedBytes, passphrase);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    // Encrypts byte array and returns encrypted byte array
    public static byte[] EncryptBytesToBytes(byte[] bytes, string passphrase)
    {
        byte[] salt = GenerateRandomSalt();
        (byte[] Key, byte[] IV) = DeriveKeyAndIV(passphrase, salt);
        byte[] encrypted;

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(bytes, 0, bytes.Length);
                }
                encrypted = msEncrypt.ToArray();
            }
        }

        // Prepend salt to encrypted data
        byte[] encryptedDataWithSalt = new byte[salt.Length + encrypted.Length];
        Buffer.BlockCopy(salt, 0, encryptedDataWithSalt, 0, salt.Length);
        Buffer.BlockCopy(encrypted, 0, encryptedDataWithSalt, salt.Length, encrypted.Length);

        return encryptedDataWithSalt;
    }

    // Decrypts encrypted byte array and returns decrypted byte array
    public static byte[] DecryptBytesToBytes(byte[] encryptedDataWithSalt, string passphrase)
    {
        // Extract salt and ciphertext from the combined array
        byte[] salt = new byte[16];
        Buffer.BlockCopy(encryptedDataWithSalt, 0, salt, 0, salt.Length);
        byte[] cipherText = new byte[encryptedDataWithSalt.Length - salt.Length];
        Buffer.BlockCopy(encryptedDataWithSalt, salt.Length, cipherText, 0, cipherText.Length);

        (byte[] Key, byte[] IV) = DeriveKeyAndIV(passphrase, salt);

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipherText))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    MemoryStream decryptedStream = new MemoryStream();
                    csDecrypt.CopyTo(decryptedStream);
                    return decryptedStream.ToArray();
                }
            }
        }
    }
}
