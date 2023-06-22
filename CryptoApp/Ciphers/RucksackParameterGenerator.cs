using System;
using System.Numerics;

namespace CryptoApp.Ciphers
{
    public static class RucksackParameterGenerator
    {
        public static BigInteger GenerateM()
        {
            //int bitLength = new Random().Next(9, 12) * 128; // 512 - 2048 bits
            BigInteger m = 0;

            /*while (!IsPrime(m) || BigInteger.Log(m, 2) < bitLength)
            {
            }*/
                BigInteger p =
                    BigInteger.Parse("109631049672404183689693967287507397169");
                BigInteger q = BigInteger.Parse("126789056546283944535895382002387649462");
                
            m = p * q;
            return m;
        }

        public static BigInteger GenerateN(BigInteger m)
        {
            BigInteger n = 0;

            while (n >= m || n == 0)
            {
                n = new BigInteger(new Random().Next(2, (int)m));
            }

            return n;
        }

        public static BigInteger GenerateT(BigInteger m)
        {
            BigInteger t = 0;

            while (t == 0 || !IsCoPrime(t, m))
            {
                t = new BigInteger(new Random().Next(2, (int)m));
            }

            return t;
        }

        private static bool IsPrime(BigInteger n)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            BigInteger i = 5;
            BigInteger w = 2;

            while (i * i <= n)
            {
                if (n % i == 0) return false;

                i += w;
                w = 6 - w;
            }

            return true;
        }

        /*private static BigInteger GeneratePrime(int bitLength)
        {
            BigInteger p = 0;
            Random rnd = new Random();

            while (!IsPrime(p))
            {
                byte[] bytes = new byte[bitLength / 8];
                rnd.NextBytes(bytes);
                bytes[bytes.Length - 1] |=
                    0x01; // Set the least significant bit to 1 to ensure oddness
                p = new BigInteger(bytes);
            }

            return p;
        }*/

        private static bool IsCoPrime(BigInteger a, BigInteger b)
        {
            return BigInteger.GreatestCommonDivisor(a, b) == 1;
        }

        public static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            a %= m;
            for (BigInteger x = BigInteger.One; x.CompareTo(m) < 0; x++)
            {
                if ((a * x) % m == BigInteger.One)
                {
                    return x;
                }
            }

            return BigInteger.One;
        }
        
        // Розширення для класу Random для генерації випадкових BigInteger
        public static class RandomExtensions
        {
            public static BigInteger NextBigInteger(Random rnd,
                BigInteger minValue, BigInteger maxValue)
            {
                var byteCount = maxValue.ToByteArray().Length;
                var rndBytes = new byte[byteCount];
                rnd.NextBytes(rndBytes);
                var value = new BigInteger(rndBytes);
                var range = maxValue - minValue;
                value %= range;
                value += minValue;
                return value;
            }
        }
    }
}