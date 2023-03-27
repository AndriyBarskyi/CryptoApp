using CryptoApp.Ciphers;
using CryptoApp.Ciphers.Alphabets;
using CryptoApp.Ciphers.CipherImpl;
using Cryptologist.Ciphers.Utils;
using Xunit;

namespace TestCiphers
{
    public class JammingCipherTests
    {
        private readonly string _key = "KEY";
        private readonly Alphabet _engAlphabet = Alphabet.English;
        private readonly Alphabet _ukrAlphabet = Alphabet.English;

        [Fact]
        public void TestEncryptDecryptEnglishText()
        {
            // Arrange
            var input = "HELLO WORLD";
            var cipher = new JammingCipher(_key, input.Length, _engAlphabet);

            // Act
            var encrypted = cipher.Encrypt(input, _engAlphabet);
            var decrypted = cipher.Decrypt(encrypted, _engAlphabet);

            // Assert
            Assert.Equal(input, decrypted);
        }
        
        [Fact]
        public void TestEncryptDecryptUkrainianText()
        {
            // Arrange
            var input = "Слава Україні!";
            var cipher = new JammingCipher(_key, input.Length, _ukrAlphabet);

            // Act
            var encrypted = cipher.Encrypt(input, _ukrAlphabet);
            var decrypted = cipher.Decrypt(encrypted, _ukrAlphabet);

            // Assert
            Assert.Equal(input, decrypted);
        }
    }
}