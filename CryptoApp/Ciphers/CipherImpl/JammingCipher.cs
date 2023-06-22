using System;
using System.Text;
using CryptoApp.Ciphers.Alphabets;

namespace CryptoApp.Ciphers.CipherImpl
{
    public class JammingCipher : Cipher
    {
        private readonly Random _random;
        private readonly string _gamma;

        public JammingCipher(string key, int inputLen, Alphabet alphabet)
        {
            TextKey = key;
            _random = new Random();
            _gamma = GenerateGamma(inputLen, TextKey, alphabet);
        }

        public override string Encrypt(string input, Alphabet alphabet)
        {
            StringBuilder encrypted = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (!char.IsLetter(c))
                {
                    encrypted.Append(c);
                    continue;
                }
                int x = alphabet.Value.IndexOf(char.ToUpper(c));
                if (x < 0) // якщо символ не знайдено в алфавіті
                {
                    encrypted.Append(c);
                    continue;
                }
                int g = alphabet.Contains(char.ToUpper(_gamma[i])) ? 
                    alphabet.Value.IndexOf(char.ToUpper(_gamma[i])) : 
                    _random.Next(alphabet.Value.Length);
                int y = (x + g) % alphabet.Value.Length;
                encrypted.Append(char.IsUpper(c) ? 
                    alphabet.Value[y] : 
                    char.ToLower(alphabet.Value[y]));
            }
            return encrypted.ToString();
        }

        public override string Decrypt(string input, Alphabet alphabet)
        {
            StringBuilder message = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (!char.IsLetter(c))
                {
                    message.Append(c);
                    continue;
                }
                int y = alphabet.Value.IndexOf(char.ToUpper(c));
                if (y < 0) // якщо символ не знайдено в алфавіті
                {
                    message.Append(c);
                    continue;
                }
                int g = alphabet.Value.IndexOf(char.ToUpper(_gamma[i]));
                int x = (y + alphabet.Value.Length - (g % alphabet.Value.Length)) % alphabet.Value.Length;
                message.Append(char.IsUpper(c) ? alphabet.Value[x] : 
                char.ToLower(alphabet.Value[x]));
            }
            return message.ToString();
        }

        public override string AttackCipher(string input, Alphabet alphabet)
        {
            return input;
        }
        
        private string GenerateGamma(int length, string key, Alphabet 
        alphabet)
        {
            int keyLength = key.Length;
            StringBuilder gamma = new StringBuilder(length);
            int i = 0;
            while (gamma.Length < length)
            {
                int k = alphabet.Value.IndexOf(char.ToUpper(key[i % keyLength]));
                int r = _random.Next(alphabet.Value.Length);
                int g = (k + r) % alphabet.Value.Length;
                gamma.Append(char.IsUpper(key[i % keyLength]) ? char.ToUpper(alphabet.Value[g]) : alphabet.Value[g]);
                i++;
            }
            return gamma.ToString();
        }
    }
}