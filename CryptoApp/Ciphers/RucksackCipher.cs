using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoApp.Ciphers
{
    public class RucksackCipher
    {
        private int[] PrivateKey { set; get; }
        private int[] PublicKey { set; get; }
        private int N { set; get; }
        private int M { set; get; }

        public RucksackCipher(int length, int seed, int n)
        {
            GeneratePrivateKey(length,
                seed); 
            M = PrivateKey.Sum() + 1;
            N = n;
            GeneratePublicKey();
        }

        private void GeneratePrivateKey(int length, int seed)
        {
            PrivateKey = GenerateSuperincreasingSequence(length, seed);
        }

        private void GeneratePublicKey()
        {
            PublicKey = new int[PrivateKey.Length];
            for (int i = 0; i < PrivateKey.Length; i++)
            {
                PublicKey[i] = PrivateKey[i] * N % M;
            }
        }

        public string Encrypt(string input)
        {
            StringBuilder output = new StringBuilder();
            foreach (var part in Parts(TextToBitsList(input),
                         PublicKey.Length))
            {
                int partsNumber = 0;
                for (int i = 0; i < part.Count; i++)
                {
                    if (part.ElementAt(i))
                    {
                        partsNumber += PublicKey[i];
                    }
                }

                output.Append(partsNumber + " ");
            }
            return output.ToString();
        }

        public string Decrypt(string input)
        {
            var invMN = ModInverse(N, M);
            var bits = new List<bool>();
            foreach (string cipherNumber in input.Trim().Split(' '))
            {
                var cipherNum = int.Parse(cipherNumber);
                var plainNum = (cipherNum * invMN) % M;
                
                var part = new List<bool>();
                for (var i = PrivateKey.Length - 1; i >= 0; i--)
                {
                    if (PrivateKey[i] <= plainNum)
                    {
                        part.Add(true);
                        plainNum -= PrivateKey[i];
                    }
                    else
                    {
                        part.Add(false);
                    }
                }
                part.Reverse();
                bits.AddRange(part);
            }
            
            return BitsListToText(bits);
        }

        private static List<bool> TextToBitsList(string text)
        {
            var bitsList = new List<bool>();

            foreach (var oneByte in Encoding.UTF8.GetBytes(text))
            {
                for (int i = 7; i >= 0; i--)
                {
                    bitsList.Add(((oneByte >> i) & 1) == 1);
                }
            }

            return bitsList;
        }

        private static string BitsListToText(List<bool> bitsList)
        {
            var byteArray = new byte[bitsList.Count / 8];
            var bitsArray =
                new BitArray(bitsList.Count - (bitsList.Count % 8));

            for (var i = 0; i < bitsArray.Length; i++)
                bitsArray[i] = bitsList.ElementAt(bitsArray.Length - i - 1);

            bitsArray.CopyTo(byteArray, 0);
            Array.Reverse(byteArray);

            return Encoding.UTF8.GetString(byteArray);
        }

        private static IEnumerable<List<bool>> Parts(List<bool> bitsLit,
            int maxChunkSize)
        {
            for (var i = 0; i < bitsLit.Count; i += maxChunkSize)
                yield return bitsLit.GetRange(i,
                    Math.Min(maxChunkSize, bitsLit.Count - i));
        }

        private int[] GenerateSuperincreasingSequence(int length, int seed)
        {
            var superincreasingSequence = new int[length];
            var rnd = new Random(seed);

            var lastSumOfPrev = 0;
            for (var i = 0; i < length; i++)
            {
                superincreasingSequence[i] =
                    rnd.Next(lastSumOfPrev, lastSumOfPrev + 10);
                lastSumOfPrev += superincreasingSequence[i];
            }
            return superincreasingSequence;

        }

        private static int ModInverse(int n, int m)
        {
            n %= m;
            for (var x = 1; x < m; x++)
                if ((n * x) % m == 1)
                    return x;
            return 1;
        }
    }
}