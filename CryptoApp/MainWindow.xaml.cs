using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CryptoApp.Ciphers;
using CryptoApp.Ciphers.Alphabets;
using CryptoApp.Ciphers.CipherImpl;
using Microsoft.Win32;

namespace CryptoApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Alphabet _alphabet = Alphabet.English;
        private byte[] _iv; // вектор ініціалізації
        private byte[] _key; // ключ шифрування
        private ICipher _selectedCipher = new CaesarCipher();

        public MainWindow()
        {
            InitializeComponent();
            SetMaxStep();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
                OutputTextBox.Text = _selectedCipher.Encrypt(InputTextBox.Text, (int)StepSlider.Value, _alphabet);
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
                OutputTextBox.Text = _selectedCipher.Decrypt(InputTextBox.Text, (int)StepSlider.Value, _alphabet);
        }

        private void BruteForceAttackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
                OutputTextBox.Text = _selectedCipher.BruteForceDecrypt(InputTextBox.Text, _alphabet);
        }

        private void CaesarMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _selectedCipher = new CaesarCipher();
        }

        private void SetMaxStep()
        {
            if (StepSlider != null) StepSlider.Maximum = _alphabet.Value.Length - 1;
        }

        private void StepSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderLabel != null && StepSlider != null) SliderLabel.Content = ((int)StepSlider.Value).ToString();
        }

        private void EnglishAlphabet_OnSelected(object sender, RoutedEventArgs e)
        {
            _alphabet = Alphabet.English;
            SetMaxStep();
        }

        private void UkrainianAlphabet_OnSelected(object sender, RoutedEventArgs e)
        {
            _alphabet = Alphabet.Ukrainian;
            SetMaxStep();
        }

        private void AboutUs_OnClick(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                var fileContent = File.ReadAllText(fileName);
                InputTextBox.Text = fileContent;
            }
        }

        private void SaveToFile_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true) File.WriteAllText(saveFileDialog.FileName, OutputTextBox.Text);
        }

        private void PrintOutput_OnClick(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var document = new FlowDocument(new Paragraph(new Run(OutputTextBox.Text)));
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Print Document");
            }
        }

        private void FrequencyTables_OnClick(object sender, RoutedEventArgs e)
        {
            var frequencyTablesWindow = new FrequencyTableWindow();
            frequencyTablesWindow.ShowDialog();
        }

        private void CipherFile_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateKeyIV();
            var openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Title = "Select a file to encrypt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Encrypted Files (*.enc)|*.enc",
                    Title = "Save encrypted file"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    var inputFile = openFileDialog.FileName;
                    var outputFile = saveFileDialog.FileName;
                    var fileEncryptor = new FileEncryptor(_key, _iv);

                    var encryptedFileName = Path.GetFileNameWithoutExtension(outputFile) + ".key";
                    File.WriteAllBytes(encryptedFileName, _key.Concat(_iv).ToArray());

                    fileEncryptor.EncryptFile(inputFile, outputFile);
                    MessageBox.Show("File encrypted successfully!");
                }
            }
        }

        private void DecipherFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Encrypted Files (*.enc)|*.enc",
                Title = "Select a file to decrypt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "All Files (*.*)|*.*",
                    Title = "Save decrypted file"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    var inputFile = openFileDialog.FileName;
                    var outputFile = saveFileDialog.FileName;

                    // Зчитування ключа і вектора ініціалізації з файлу ініціалізації
                    var encryptedFileName = Path.GetFileNameWithoutExtension(inputFile) + ".key";
                    var keyIvBytes = File.ReadAllBytes(encryptedFileName);
                    _key = keyIvBytes.Take(32).ToArray();
                    _iv = keyIvBytes.Skip(32).ToArray();

                    var fileEncryptor = new FileEncryptor(_key, _iv);
                    fileEncryptor.DecryptFile(inputFile, outputFile);
                    MessageBox.Show("File decrypted successfully!");
                }
            }
        }

        private void GenerateKeyIV()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                _key = aes.Key;
                _iv = aes.IV;
            }
        }

        private void SaveKeysToFile(byte[] key, byte[] iv)
        {
            File.WriteAllBytes("key.bin", key);
            File.WriteAllBytes("iv.bin", iv);
        }

        private byte[] LoadKeyFromFile()
        {
            return File.ReadAllBytes("key.bin");
        }

        private byte[] LoadIvFromFile()
        {
            return File.ReadAllBytes("iv.bin");
        }
    }
}