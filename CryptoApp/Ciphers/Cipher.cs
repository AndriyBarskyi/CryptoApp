using CryptoApp.Ciphers.Alphabets;

namespace CryptoApp.Ciphers
{
    public abstract class Cipher
    {
        public int Key { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public abstract string Encrypt(string input, Alphabet alphabet);
        public abstract string Decrypt(string input, Alphabet alphabet);

        public abstract string AttackCipher(string input, Alphabet alphabet);
    }
}