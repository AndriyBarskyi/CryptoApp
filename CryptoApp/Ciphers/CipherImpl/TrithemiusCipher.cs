using System;
using System.Text;
using CryptoApp.Ciphers.Alphabets;

namespace CryptoApp.Ciphers.CipherImpl
{
    public class TrithemiusCipher : Cipher
    {
        public override string Encrypt(string input, Alphabet alphabet)
        {
            var output = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                if (alphabet.Contains(input[i]))
                {
                    if (C == 0)
                    {
                        Key = CalculateK(A, B, i);
                    }
                    else if (A != 0 && B != 0 && C != 0)
                    {
                        Key = CalculateK(A, B, C, i);
                    }

                    var newChar =
                        alphabet.Value[(alphabet.Value.IndexOf(input[i]
                                            .ToString().ToUpper(), StringComparison.Ordinal) + 
                                        Key) % 
                        alphabet
                        .Value.Length];
                    output.Append(char.IsLower(input[i]) ||
                                  char.IsWhiteSpace
                                      (input[i])
                        ? char.ToLower(newChar)
                        : newChar);
                }
                else
                {
                    output.Append(input[i]);
                }
            }

            return output.ToString();
        }

        public override string Decrypt(string input, Alphabet alphabet)
        {
            var output = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                if (alphabet.Contains(input[i]))
                {
                    if (C == 0)
                    {
                        Key = CalculateK(A, B, i);
                    }
                    else if (A != 0 && B != 0 && C != 0)
                    {
                        Key = CalculateK(A, B, C, i);
                    }
                    var newChar =
                        alphabet.Value[((alphabet.Value.IndexOf(input[i].ToString().ToUpper(), StringComparison.Ordinal) + alphabet.Value.Length - (Key %
                            alphabet
                                .Value.Length)) % alphabet.Value.Length)];
                    output.Append(char.IsLower(input[i]) ||
                                  char.IsWhiteSpace(input[i])
                        ? char.ToLower(newChar)
                        : newChar);
                }
                else
                {
                    output.Append(input[i]);
                }
            }

            return output.ToString();
        }


        public override string AttackCipher(string input, Alphabet alphabet)
        {
            throw new NotImplementedException();
        }

        private static int CalculateK(int A, int B, int charPos)
        {
            return A * charPos + B;
        }

        private static int CalculateK(int A, int B, int C, int charPos)
        {
            return A * (int)Math.Pow(charPos, 2) + B * charPos + C;
        }
    }
}