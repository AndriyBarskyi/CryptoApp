using System.Collections.Generic;
using System.Linq;
using CryptoApp.Ciphers.Alphabets;
using Cryptologist.Ciphers.Utils;

namespace CryptoApp
{
    public class FrequencyTable
    {
        private readonly Dictionary<char, double> _frequencies;
        public FrequencyTable()
        {
            _frequencies = new Dictionary<char, double>();
        }

        public void CalculateFrequencies(string text, Alphabet alphabet)
        {
            int totalChars = text.Count(alphabet.Contains);
            foreach (char c in text)
            {
                if (!alphabet.Contains(c))
                    continue;

                char upperC = char.ToUpper(c);
                if (!_frequencies.ContainsKey(upperC))
                    _frequencies[upperC] = 0;

                _frequencies[upperC] += 1.0 / totalChars;
            }
        }
        
        public Dictionary<char, double> GetFrequencies()
        {
            return _frequencies;
        }
    }
}