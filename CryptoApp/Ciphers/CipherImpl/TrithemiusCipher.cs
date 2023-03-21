using System;
using System.Collections.Generic;
using System.Linq;
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
                                                .ToString().ToUpper(),
                                            StringComparison.Ordinal) +
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
                        alphabet.Value[
                            ((alphabet.Value.IndexOf(
                                     input[i].ToString().ToUpper(),
                                     StringComparison.Ordinal) +
                                 alphabet.Value.Length - (Key %
                                     alphabet
                                         .Value.Length)) %
                             alphabet.Value.Length)];
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
            var possibleKeys = new List<Tuple<int, int, int>>();

            for (int A = 1; A < alphabet.Value.Length; A++)
            {
                for (int B = 0; B < alphabet.Value.Length; B++)
                {
                    for (int C = 0; C < alphabet.Value.Length; C++)
                    {
                        var decrypted = new TrithemiusCipher
                                { A = A, B = B, C = C }
                            .Decrypt(input, alphabet);

                        double score = CalculateScore(
                            decrypted, alphabet);

                        if (score < 0.4)
                        {
                            possibleKeys.Add(new Tuple<int, int, int>(A, B, C));
                        }
                    }
                }
            }

            if (possibleKeys.Count == 0)
            {
                return "No possible keys found.";
            }

            var output = new StringBuilder();
            output.AppendLine("Possible decryptions:");
            foreach (var key in possibleKeys)
            {
                TrithemiusCipher cipher = new TrithemiusCipher();
                cipher.A = key.Item1;
                cipher.B = key.Item2;
                cipher.C = key.Item3;
                output.AppendLine("--------------------------------------");
                output.AppendLine(cipher.Decrypt(input, alphabet));
            }

            return output.ToString();
        }

        public string AttackCipherKeys(string cipherText, string plainText,
            Alphabet alphabet)
        {
            var possibleKeys = new List<Tuple<int, int, int>>();

            for (int A = 1; A < alphabet.Value.Length; A++)
            {
                for (int B = 0; B < alphabet.Value.Length; B++)
                {
                    for (int C = 0; C < alphabet.Value.Length; C++)
                    {
                        var decrypted = new TrithemiusCipher
                                { A = A, B = B, C = C }
                            .Decrypt(cipherText, alphabet);

                        if (plainText.Equals(decrypted))
                        {
                            possibleKeys.Add(new Tuple<int, int, int>(A, B, C));
                            break;
                        }
                    }
                }
            }

            if (possibleKeys.Count == 0)
            {
                return "No possible keys found.";
            }

            var output = new StringBuilder();
            output.AppendLine("Possible keys:");
            foreach (var key in possibleKeys)
            {
                output.AppendLine(
                    $"A: {key.Item1}, B: {key.Item2}, C: {key.Item3}");
            }

            return output.ToString();
        }

        private double CalculateScore(string decrypted, Alphabet alphabet)
        {
            var freqTable = new FrequencyTable();
            freqTable.CalculateFrequencies(decrypted, alphabet);
            var frequencies = freqTable.GetFrequencies();

            return (from freq in frequencies
                let expectedFreq = FrequencyTable.GetFrequencies()[freq.Key]
                select Math.Abs(expectedFreq - freq.Value)).Sum();
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