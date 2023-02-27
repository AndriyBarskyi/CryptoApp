using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Cryptologist.Ciphers;
using Cryptologist.Ciphers.Utils;
using Microsoft.Win32;

namespace CryptoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ICipher _selectedCipher = new CaesarCipher();
        private Alphabet _alphabet = Alphabet.English;
        public MainWindow()
        {
            InitializeComponent(); 
            SetMaxStep();
        }
        
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                OutputTextBox.Text = _selectedCipher.Encrypt(InputTextBox.Text, (int)StepSlider.Value, _alphabet);
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                OutputTextBox.Text = _selectedCipher.Decrypt(InputTextBox.Text, (int)StepSlider.Value, _alphabet);
            }
        }
        
        private void CaesarMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _selectedCipher = new CaesarCipher();
        }

        private void SetMaxStep()
        {
            if (StepSlider != null)
            {
                StepSlider.Maximum = _alphabet.Value.Length - 1;
            }
        }

        private void StepSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        { 
            if (SliderLabel != null && StepSlider != null)
            {
                SliderLabel.Content = ((int)StepSlider.Value).ToString();
            }
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
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string fileContent = File.ReadAllText(fileName);
                InputTextBox.Text = fileContent;
            }
        }

        private void SaveToFile_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, OutputTextBox.Text);
            }
        }

        private void PrintOutput_OnClick(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument document = new FlowDocument(new Paragraph(new Run(OutputTextBox.Text)));
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Print Document");
            }
        }
    }
}