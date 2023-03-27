using System;
using System.Security.Cryptography;
using System.Text;
using CryptoApp.Ciphers.Alphabets;

namespace CryptoApp.Ciphers.CipherImpl
{
    public class VernamCipher : Cipher
    {
        private int KeyLength { get; }
        private byte[] key;

        public VernamCipher(int keyLength)
        {
            KeyLength = keyLength;
            GenerateKey();
        }

        private void GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                key = new byte[KeyLength];
                rng.GetBytes(key);
            }
        }

        public override string Encrypt(string input, Alphabet alphabet)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var encryptedData = new byte[data.Length];
            for (var j = 0; j < KeyLength; j++)
            {
                encryptedData[j] = (byte)(data[j] ^ key[j]);
            }
            return Convert.ToBase64String(encryptedData);
        }

        public override string Decrypt(string input, Alphabet alphabet)
        {
            var data = Convert.FromBase64String(input);
            var decryptedData = new byte[data.Length];
            for (var j = 0; j < KeyLength; j++)
            {
                decryptedData[j] = (byte)(data[j] ^ key[j]);
            }
            return Encoding.UTF8.GetString(decryptedData);
        }

        public override string AttackCipher(string input, Alphabet alphabet)
        {
            return input;
        }
    }
}