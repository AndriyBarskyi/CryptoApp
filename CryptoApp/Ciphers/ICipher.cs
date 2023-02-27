using Cryptologist.Ciphers.Utils;

namespace Cryptologist.Ciphers
{
    public interface ICipher
    {
        string Encrypt(string input, int key, Alphabet alphabet);
        string Decrypt(string input, int key, Alphabet alphabet);
    }
}