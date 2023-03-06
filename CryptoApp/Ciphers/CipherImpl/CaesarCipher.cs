using System.Collections.Generic;
using System.Text;
using CryptoApp.Ciphers.Alphabets;
using Cryptologist.Ciphers;
namespace CryptoApp.Ciphers.CipherImpl
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
        
        public string BruteForceDecrypt(string input, Alphabet alphabet)
        {
            var possibleDecryptions = new List<string>();
            DictionaryChecker dictionaryChecker = new DictionaryChecker();
            for (int i = 0; i < alphabet.Value.Length; i++)
            {
                string[] inputWords = input.Split();
                for (int j = 0; j < (inputWords.Length < 10 ? inputWords.Length : 10); j++)
                {
                    if (alphabet.Equals(Alphabet.English) && inputWords[j].Length > 3 && dictionaryChecker.CheckIfWordExists(Decrypt(inputWords[j], i, alphabet)).Result)
                    {
                        possibleDecryptions.Add(Decrypt(input, i, alphabet));
                        break;
                    }
                    if (!alphabet.Equals(Alphabet.English))
                    {
                        possibleDecryptions.Add(Decrypt(input, i, alphabet));
                        break;
                    }
                }

            }
            return string.Join("\n-------------------------------\n", possibleDecryptions);
        }
    }
}