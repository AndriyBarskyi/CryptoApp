using System.Collections.Generic;
using System.Linq;
using CryptoApp;
using CryptoApp.Ciphers;
using CryptoApp.Ciphers.Alphabets;
using Cryptologist.Ciphers.Utils;
using Xunit;

namespace TestCiphers
{
    public class FrequencyTableTests
    {
        [Fact]
        public void CalculateFrequencies_ShouldCalculateFrequenciesForEnglishAlphabet()
        {
// Arrange
            var frequencyTable = new FrequencyTable();
            var alphabet = Alphabet.English;
            var text = "Hello, world!";
            var lettersCount = text.Count(alphabet.Contains);
            var expectedFrequencies = new Dictionary<char, double>
            {
                { 'H', 1.0 / lettersCount },
                { 'E', 1.0 / lettersCount },
                { 'L', 3.0 / lettersCount },
                { 'O', 2.0 / lettersCount },
                { 'W', 1.0 / lettersCount },
                { 'R', 1.0 / lettersCount },
                { 'D', 1.0 / lettersCount },
            };
            // Act
            frequencyTable.CalculateFrequencies(text, Alphabet.English);
            var actualFrequencies = frequencyTable.GetFrequencies();

            // Assert
            // Assert
            foreach (var pair in expectedFrequencies)
            {
                Assert.Equal(pair.Value, actualFrequencies[pair.Key], 4);
            }
        }

        [Fact]
        public void CalculateFrequencies_ShouldCalculateFrequenciesForUkrainianAlphabet()
        {
            // Arrange
            var frequencyTable = new FrequencyTable();
            var alphabet = Alphabet.Ukrainian;
            var text = "Це український текст.";
            var lettersCount = text.Count(alphabet.Contains);
            var expectedFrequencies = new Dictionary<char, double>
            {
                { 'Ц', 1.0 / lettersCount },
                { 'Е', 2.0 / lettersCount },
                { 'У', 1.0 / lettersCount },
                { 'К', 3.0 / lettersCount },
                { 'Р', 1.0 / lettersCount },
                { 'А', 1.0 / lettersCount },
                { 'Ї', 1.0 / lettersCount },
                { 'Н', 1.0 / lettersCount },
                { 'С', 2.0 / lettersCount },
                { 'Ь', 1.0 / lettersCount },
                { 'И', 1.0 / lettersCount },
                { 'Й', 1.0 / lettersCount },
                { 'Т', 2.0 / lettersCount },
            };

            // Act
            frequencyTable.CalculateFrequencies(text, Alphabet.Ukrainian);
            var actualFrequencies = frequencyTable.GetFrequencies();

            // Assert
            foreach (var pair in expectedFrequencies)
            {
                Assert.Equal(pair.Value, actualFrequencies[pair.Key], 4);
            }
        }
    }
}