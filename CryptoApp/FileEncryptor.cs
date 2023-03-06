using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using AForge.Imaging.Filters;

namespace CryptoApp
{
    public class FileEncryptor
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public FileEncryptor(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }

        public void EncryptFile(string inputFile, string outputFile)
        {
            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(_key, _iv))
            using (var cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
            {
                aes.KeySize = _key.Length * 8;
                aes.BlockSize = _iv.Length * 8;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }

        public void DecryptFile(string inputFile, string outputFile)
        {
            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var aes = Aes.Create())
            using (var decryptor = aes.CreateDecryptor(_key, _iv))
            using (var cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
            {
                aes.KeySize = _key.Length * 8;
                aes.BlockSize = _iv.Length * 8;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputFileStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}