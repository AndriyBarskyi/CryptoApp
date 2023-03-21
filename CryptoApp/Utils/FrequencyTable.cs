using System.Collections.Generic;
using System.Linq;
using CryptoApp.Ciphers.Alphabets;

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
            var totalChars = text.Count(alphabet.Contains);
            foreach (var c in text)
            {
                if (!alphabet.Contains(c))
                    continue;

                var upperC = char.ToUpper(c);
                if (!_frequencies.ContainsKey(upperC))
                    _frequencies[upperC] = 0;

                _frequencies[upperC] += 1.0 / totalChars;
            }
        }

        public Dictionary<char, double> GetFrequencies()
        {
            return _frequencies.OrderByDescending(x => x.Value).ToDictionary
            (x => x.Key, x => x.Value);
        }
    }
}