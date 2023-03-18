using CryptoApp.Ciphers;
using CryptoApp.Ciphers.Alphabets;
using CryptoApp.Ciphers.CipherImpl;
using Cryptologist.Ciphers.Utils;
using Xunit;

namespace TestCiphers
{
    public class TrithemiusCipherTests
        {
            [Fact]
            public void Encrypt_ValidInput_ReturnsExpectedResult()
            {
                // Arrange
                var cipher = new TrithemiusCipher
                {
                    A = 1,
                    B = 5,
                    C = 8
                };
                var input = "Hello, world!";
                var alphabet = Alphabet.English;

                // Act
                var result = cipher.Encrypt(input, alphabet);

                // Assert
                Assert.Equal("Pshrg, kwvnf!", result);
            }
            
            [Fact]
            public void Decrypt_ValidInput_ReturnsExpectedResult()
            {
                // Arrange
                var cipher = new TrithemiusCipher
                {
                    A = 2,
                    B = 4, 
                    C = 0
                };
                const string input = "EHKNQTWZCFILORUXADGJMPSVYB";
                var alphabet = Alphabet.English;

                // Act
                var result = cipher.Decrypt(input, alphabet);

                // Assert
                Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", result);
            }

            [Fact]
            public void Encrypt_WithUkrainianText_ReturnsCorrectResult()
            {
                const string input = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                const string expectedOutput = "ГЗЛСЧАЕЇОФЬГЗЛСЧАЕЇОФЬГЗЛСЧАЕЇОФЬ";
                var alphabet = Alphabet.Ukrainian;
                var cipher = new TrithemiusCipher
                {
                    A = 5,
                    B = 3,
                    C = 0
                };

                var output = cipher.Encrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }

            [Fact]
            public void Decrypt_WithUkrainianText_ReturnsCorrectResult()
            {
                const string input = "Чьфвз Ннясзуц!";
                const string expectedOutput = "Слава Україні!";
                var alphabet = Alphabet.Ukrainian;
                var cipher = new TrithemiusCipher
                {
                    A = 9,
                    B = 6,
                    C = 0
                };

                var output = cipher.Decrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }

            [Fact]
            public void Encrypt_WithEmptyInput_ReturnsEmptyString()
            {
                const string input = "";
                const string expectedOutput = "";
                var alphabet = Alphabet.English;
                var cipher = new TrithemiusCipher
                {
                    Key = 3
                };

                var output = cipher.Encrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }

            [Fact]
            public void Decrypt_WithEmptyInput_ReturnsEmptyString()
            {
                const string input = "";
                const string expectedOutput = "";
                var alphabet = Alphabet.English;
                var cipher = new TrithemiusCipher
                {
                    Key = 3
                };

                var output = cipher.Decrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }
        }
}