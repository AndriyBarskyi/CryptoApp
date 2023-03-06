using System.Collections.Generic;
using System.Text;
using CryptoApp.Ciphers.Alphabets;

namespace CryptoApp.Ciphers.CipherImpl
{
    public class CaesarCipher : ICipher
    {
        public string Encrypt(string input, int key, Alphabet alphabet)
        {
            var output = new StringBuilder(input.Length);
            foreach (var c in input)
            {
                var index = alphabet.Value.IndexOf(char.ToUpper(c));
                if (index >= 0)
                {
                    var newChar = alphabet.Value[(index + key) % alphabet.Value.Length];
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
            var dictionaryChecker = new DictionaryChecker();
            for (var i = 0; i < alphabet.Value.Length; i++)
            {
                var inputWords = input.Split();
                for (var j = 0; j < (inputWords.Length < 10 ? inputWords.Length : 10); j++)
                {
                    if (alphabet.Equals(Alphabet.English) && inputWords[j].Length > 3 && dictionaryChecker
                            .CheckIfWordExists(Decrypt(inputWords[j], i, alphabet)).Result)
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