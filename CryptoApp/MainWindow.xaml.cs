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
    public partial class MainWindow : Window
    {
        private Alphabet _alphabet = Alphabet.English;
        private byte[] _iv; // вектор ініціалізації
        private byte[] _aesKey; // ключ шифрування
        private Cipher _selectedCipher = new CaesarCipher();
        private int _key;
        private bool isVernam = false;

        public MainWindow()
        {
            InitializeComponent();
            SetMaxStep();
        }
        public delegate void SetOutputTextDelegate(string text);

        private void StepSlider_OnValueChanged(object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (CaesarGroupBox != null && StepSlider != null)
            {
                _key = (int)StepSlider.Value;
                CaesarGroupBox.Header = "Step: " + (int)StepSlider.Value;
            }
        }

        #region Encrypt, Decrypt, Attack Onclicks

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InputTextBox.Text)) return;
            if (isVernam)
            {
                _selectedCipher = new VernamCipher(InputTextBox.Text.Length);
            }
            _selectedCipher.Key = _key;
            OutputTextBox.Text =
                _selectedCipher.Encrypt(InputTextBox.Text, _alphabet);
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InputTextBox.Text)) return;
            _selectedCipher.Key = _key;
            OutputTextBox.Text =
                _selectedCipher.Decrypt(InputTextBox.Text, _alphabet);
        }

        private void AttackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text) &&
                TrithemiusGroupBox.Visibility != Visibility.Visible)
            {
                OutputTextBox.Text =
                    _selectedCipher.AttackCipher(InputTextBox.Text, _alphabet);
            }
            else if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                var frequencyTablesWindow = new FrequencyTableWindow(
                    (TrithemiusCipher)_selectedCipher, (outputText) =>
                        SetOutputText
                            (outputText), InputTextBox.Text);
                frequencyTablesWindow.ShowDialog();
            }
        }

        private void SetOutputText(string text)
        {
            OutputTextBox.Text = text;
        }

        #endregion

        #region Cipher menu items OnClicks

        private void CaesarMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _selectedCipher = new CaesarCipher();
            CaesarGroupBox.Visibility = Visibility.Visible;
            TrithemiusGroupBox.Visibility = Visibility.Collapsed;
            TrithemiusActiveAttackButton.Visibility = Visibility.Collapsed;
            BruteForceAttackButton.Visibility = Visibility.Visible;
            JammingGroupBox.Visibility = Visibility.Collapsed;
        }

        private void TrithemiusMenuItem_OnClick(object sender,
            RoutedEventArgs e)
        {
            _selectedCipher = new TrithemiusCipher();
            CaesarGroupBox.Visibility = Visibility.Collapsed;
            TrithemiusGroupBox.Visibility = Visibility.Visible;
            TrithemiusActiveAttackButton.Visibility = Visibility.Visible;
            BruteForceAttackButton.Visibility = Visibility.Visible;
            JammingGroupBox.Visibility = Visibility.Collapsed;
        }

        private void JammingMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            CaesarGroupBox.Visibility = Visibility.Collapsed;
            TrithemiusGroupBox.Visibility = Visibility.Collapsed;
            TrithemiusActiveAttackButton.Visibility = Visibility.Collapsed;
            BruteForceAttackButton.Visibility = Visibility.Collapsed;
            JammingGroupBox.Visibility = Visibility.Visible;
        }
        
        private void VernamCipher_OnClick(object sender, RoutedEventArgs e)
        {
            isVernam = true;
            CaesarGroupBox.Visibility = Visibility.Collapsed;
            TrithemiusGroupBox.Visibility = Visibility.Collapsed;
            TrithemiusActiveAttackButton.Visibility = Visibility.Collapsed;
            BruteForceAttackButton.Visibility = Visibility.Collapsed;
            JammingGroupBox.Visibility = Visibility.Collapsed;
        }

        #endregion
        
        #region Alphabet selection

        private void EnglishAlphabet_OnSelected(object sender,
            RoutedEventArgs e)
        {
            _alphabet = Alphabet.English;
            SetMaxStep();
        }

        private void UkrainianAlphabet_OnSelected(object sender,
            RoutedEventArgs e)
        {
            _alphabet = Alphabet.Ukrainian;
            SetMaxStep();
        }

        private void SetMaxStep()
        {
            if (StepSlider != null)
                StepSlider.Maximum = _alphabet.Value.Length - 1;
        }

        #endregion

        #region Menu bar buttons

        private void AboutUs_OnClick(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FrequencyTables_OnClick(object sender, RoutedEventArgs e)
        {
            var frequencyTablesWindow = new FrequencyTableWindow(
                (TrithemiusCipher)_selectedCipher, (outputText) => SetOutputText
                    (outputText), InputTextBox.Text);
            frequencyTablesWindow.ShowDialog();
        }

        #endregion

        #region Work with files

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
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, OutputTextBox.Text);
        }

        private void PrintOutput_OnClick(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var document =
                    new FlowDocument(
                        new Paragraph(new Run(OutputTextBox.Text)));
                printDialog.PrintDocument(
                    ((IDocumentPaginatorSource)document).DocumentPaginator,
                    "Print Document");
            }
        }

        private void CipherFile_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateKeyIv();
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
                    var fileEncryptor = new FileEncryptor(_aesKey, _iv);

                    var encryptedFileName =
                        Path.GetFileNameWithoutExtension(outputFile) + ".key";
                    File.WriteAllBytes(encryptedFileName,
                        _aesKey.Concat(_iv).ToArray());

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
                    var encryptedFileName =
                        Path.GetFileNameWithoutExtension(inputFile) + ".key";
                    var keyIvBytes = File.ReadAllBytes(encryptedFileName);
                    _aesKey = keyIvBytes.Take(32).ToArray();
                    _iv = keyIvBytes.Skip(32).ToArray();

                    var fileEncryptor = new FileEncryptor(_aesKey, _iv);
                    fileEncryptor.DecryptFile(inputFile, outputFile);
                    MessageBox.Show("File decrypted successfully!");
                }
            }
        }

        private void GenerateKeyIv()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                _aesKey = aes.Key;
                _iv = aes.IV;
            }
        }

        #endregion

        #region Trithemius key selection, window update and attack

        private void TrithemiusLinearQ_OnSelected
            (object sender, RoutedEventArgs e)
        {
            if (!TrithemiusGroupBox.IsVisible) return;
            TrithemiusLinearQInputA.Visibility = Visibility.Visible;
            ALabel.Visibility = Visibility.Visible;
            TrithemiusLinearQInputB.Visibility = Visibility.Visible;
            BLabel.Visibility = Visibility.Visible;
            TrithemiusNonlinearQInputC.Visibility = Visibility.Collapsed;
            CLabel.Visibility = Visibility.Collapsed;
            TrithemiusWordKeyInput.Visibility = Visibility.Collapsed;
        }

        private void TrithemiusNonLinearQ_OnSelected
            (object sender, RoutedEventArgs e)
        {
            if (!TrithemiusGroupBox.IsVisible) return;
            TrithemiusLinearQInputA.Visibility = Visibility.Visible;
            ALabel.Visibility = Visibility.Visible;
            TrithemiusLinearQInputB.Visibility = Visibility.Visible;
            BLabel.Visibility = Visibility.Visible;
            TrithemiusNonlinearQInputC.Visibility = Visibility.Visible;
            CLabel.Visibility = Visibility.Visible;
            TrithemiusWordKeyInput.Visibility = Visibility.Collapsed;
        }


        private void TrithemiusKeyWord_OnSelected(object sender,
            RoutedEventArgs e)
        {
            if (!TrithemiusGroupBox.IsVisible) return;
            TrithemiusLinearQInputA.Visibility = Visibility.Collapsed;
            ALabel.Visibility = Visibility.Collapsed;
            TrithemiusLinearQInputB.Visibility = Visibility.Collapsed;
            BLabel.Visibility = Visibility.Collapsed;
            TrithemiusNonlinearQInputC.Visibility = Visibility.Collapsed;
            CLabel.Visibility = Visibility.Collapsed;
            TrithemiusWordKeyInput.Visibility = Visibility.Visible;
        }


        private void TrithemiusInput_OnValueChanged(object sender,
            RoutedPropertyChangedEventArgs<object> e)
        {
            if (TrithemiusLinearQInputA.IsVisible &&
                TrithemiusLinearQInputB.IsVisible &&
                TrithemiusNonlinearQInputC.IsVisible)
            {
                _selectedCipher.A =
                    (TrithemiusLinearQInputA.Value).GetValueOrDefault();
                _selectedCipher.B =
                    (TrithemiusLinearQInputB.Value).GetValueOrDefault();
                _selectedCipher.C = (TrithemiusNonlinearQInputC.Value)
                    .GetValueOrDefault();
            }
            else if (TrithemiusLinearQInputA.IsVisible &&
                     TrithemiusLinearQInputB.IsVisible &&
                     !TrithemiusNonlinearQInputC.IsVisible)
            {
                _selectedCipher.A =
                    (TrithemiusLinearQInputA.Value).GetValueOrDefault();
                _selectedCipher.B =
                    (TrithemiusLinearQInputB.Value).GetValueOrDefault();
            }
        }

        private void TrithemiusWordKeyInput_OnTextChanged(object sender,
            TextChangedEventArgs e)
        {
            if (TrithemiusWordKeyInput.IsVisible && !string.IsNullOrEmpty
                    (TrithemiusWordKeyInput.Text))
            {
                _selectedCipher.Key = TrithemiusWordKeyInput.Text.GetHashCode();
            }
        }

        private void TrithemiusActiveAttackButton_OnClick(object sender,
            RoutedEventArgs e)
        {
            ActiveAttackWindow attackWindow = new ActiveAttackWindow
                (InputTextBox.Text, OutputTextBox.Text, _alphabet);
            attackWindow.ShowDialog();
        }

        #endregion

        private void JammingWordKeyInput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(JammingWordKeyInput.Text) ||
                string.IsNullOrEmpty(InputTextBox.Text)) return;
            _selectedCipher = new JammingCipher(JammingWordKeyInput.Text,
                InputTextBox.Text.Length, _alphabet);
        }
    }
}