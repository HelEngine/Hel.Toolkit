using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Hel.Toolkit.Encryption
{
    /// <summary>
    /// Hel utilities for encrypting and decrypting files for the purpose of saving and loading files
    /// HelAes uses Aes256
    /// </summary>
    public class HelAes
    {
        
        /// <summary>
        /// Generate a key from the provided string. This should not be considered very secure. If security is paramount,
        /// you should consider manually generating a key.
        /// </summary>
        /// <param name="text">Password to generate key from</param>
        /// <returns>AES256 key</returns>
        public static byte[] GenerateKeyFromString(string text)
        {
            const int iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(text, new byte[] { 10, 20, 30 , 40, 50, 60, 70, 80}, iterations);
            return keyGenerator.GetBytes(32);
        }
        
        /// <summary>
        /// Encrypts a string (JSON for example) using AES256. Must provide a key to encrypt and that key must be
        /// used to decrypt later on. <see cref="GenerateKeyFromString"/> may be used to generate a key.
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>Encrypted text as byte array</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static byte[] EncryptStringToBytes(string plainText, byte[] Key)
        {
            // Create an Aes object
            // with the specified key and IV.
            using var aesAlg = Aes.Create();
            
            if (aesAlg == null) throw new NullReferenceException();
            aesAlg.GenerateIV();
            aesAlg.Key = Key;

            // Create an encryptor to perform the stream transform.
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            msEncrypt.Write(aesAlg.IV);

            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                //Write all data to the stream.
                swEncrypt.Write(plainText);
            }
            
            var encrypted = msEncrypt.ToArray();

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Decrypt a byte array into a string with the provided key.
        /// Key can be generated from <see cref="GenerateKeyFromString"/>
        /// </summary>
        /// <param name="cipherText">Encrypted text</param>
        /// <param name="Key">Generated key</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key)
        {
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            //get first 16 bytes of IV and use it to decrypt
            var iv = new byte[16];
            Array.Copy(cipherText, 0, iv, 0, iv.Length);
            cipherText = cipherText.Skip(16).ToArray();
            
            // Create an Aes object
            // with the specified key and IV.
            using Aes aesAlg = Aes.Create();
            if (aesAlg == null) throw new NullReferenceException();
            aesAlg.Key = Key;
            aesAlg.IV = iv;

            // Create a decryptor to perform the stream transform.
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
                
            // Read the decrypted bytes from the decrypting stream
            // and place them in a string.
            plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }
    }
}