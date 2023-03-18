using CryptoApp.Ciphers;
using CryptoApp.Ciphers.Alphabets;
using CryptoApp.Ciphers.CipherImpl;
using Cryptologist.Ciphers.Utils;
using Xunit;

namespace TestCiphers
{
    public class CaesarCipherTests
        {
            [Fact]
            public void Encrypt_ShiftOne_ReturnsCorrectResult()
            {
                // Arrange
                var cipher = new CaesarCipher();
                cipher.Key = 1;
                var input = "HELLO";
                var alphabet = Alphabet.English;

                // Act
                var result = cipher.Encrypt(input, alphabet);

                // Assert
                Assert.Equal("IFMMP", result);
            }
            
            [Fact]
            public void Decrypt_ShiftOne_ReturnsCorrectResult()
            {
                // Arrange
                var cipher = new CaesarCipher();
                var input = "IFMMP";
                cipher.Key = 1;
                var alphabet = Alphabet.English;

                // Act
                var result = cipher.Decrypt(input, alphabet);

                // Assert
                Assert.Equal("HELLO", result);
            }

            [Fact]
            public void Encrypt_WithUkrainianText_ReturnsCorrectResult()
            {
                string input = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                string expectedOutput = "ГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯАБВ";
                Alphabet alphabet = Alphabet.Ukrainian;
                CaesarCipher cipher = new CaesarCipher();
                cipher.Key = 3;

                string output = cipher.Encrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }

            [Fact]
            public void Decrypt_WithUkrainianText_ReturnsCorrectResult()
            {
                string input = "ГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯАБВ";
                string expectedOutput = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                Alphabet alphabet = Alphabet.Ukrainian;
                CaesarCipher cipher = new CaesarCipher();
                cipher.Key = 3;

                string output = cipher.Decrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }

            [Fact]
            public void Encrypt_WithEmptyInput_ReturnsEmptyString()
            {
                string input = "";
                string expectedOutput = "";
                Alphabet alphabet = Alphabet.English;
                CaesarCipher cipher = new CaesarCipher();
                cipher.Key = 3;

                string output = cipher.Encrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }

            [Fact]
            public void Decrypt_WithEmptyInput_ReturnsEmptyString()
            {
                string input = "";
                string expectedOutput = "";
                Alphabet alphabet = Alphabet.English;
                CaesarCipher cipher = new CaesarCipher();
                cipher.Key = 3;

                string output = cipher.Decrypt(input, alphabet);

                Assert.Equal(expectedOutput, output);
            }
        }
}