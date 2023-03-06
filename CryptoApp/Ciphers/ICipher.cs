using System.Collections.Generic;
using CryptoApp.Ciphers.Alphabets;
using Cryptologist.Ciphers.Utils;

namespace CryptoApp.Ciphers
{
    public interface ICipher
    {
        string Encrypt(string input, int key, Alphabet alphabet);
        string Decrypt(string input, int key, Alphabet alphabet);

        string BruteForceDecrypt(string input, Alphabet alphabet);
    }
}