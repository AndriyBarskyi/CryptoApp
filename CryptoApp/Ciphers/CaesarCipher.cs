using System.Text;
using Cryptologist.Ciphers.Utils;

namespace Cryptologist.Ciphers
{
    public class CaesarCipher : ICipher
    {
        public string Encrypt(string input, int key, Alphabet alphabet)
        {
            StringBuilder output = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                int index = alphabet.Value.IndexOf(char.ToUpper(c));
                if (index >= 0)
                {
                    char newChar = alphabet.Value[(index + key) % alphabet.Value.Length];
                    output.Append(char.IsLower(c) ? char.ToLower(newChar) : newChar);
                }
                else
                {
                    output.Append(c);
                }
            }
            return output.ToString();
        }

        public string Decrypt(string input, int key, Alphabet alphabet)
        {
            return Encrypt(input, alphabet.Value.Length - key, alphabet);
        }
    }
}