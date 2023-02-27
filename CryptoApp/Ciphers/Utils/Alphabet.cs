using Cryptologist.Ciphers.Utils;

namespace Cryptologist.Ciphers.Utils
{
    public class Alphabet : Enumeration
    {
        public static readonly Alphabet English 
            = new Alphabet(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        public static readonly Alphabet Ukrainian 
            = new Alphabet(2, "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ");

        private Alphabet(int id, string alphabet)
            : base(id, alphabet)
        {
        }
    }
}