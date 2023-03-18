using System;
using Cryptologist.Ciphers.Utils;

namespace CryptoApp.Ciphers.Alphabets
{
    public class Alphabet : Enumeration
    {
        public static readonly Alphabet English
            = new Alphabet("eng", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

        public static readonly Alphabet Ukrainian
            = new Alphabet("ukr", "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ");

        private Alphabet(string code, string alphabet)
            : base(code, alphabet)
        {
        }

        public bool Contains(char c)
        {
            return Value.IndexOf(c.ToString().ToUpper(), StringComparison.Ordinal) != -1;
        }
    }
}