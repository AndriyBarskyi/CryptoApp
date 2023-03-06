using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CryptoApp.Ciphers.Alphabets;
using Microsoft.Win32;

namespace CryptoApp
{
    public partial class FrequencyTableWindow : Window
    {
        private Alphabet _alphabet = Alphabet.English;
        private FrequencyTable _frequencyTable;

        public FrequencyTableWindow()
        {
            InitializeComponent();

            EnglishRadioButton.Checked += OnCheckedChanged;
            UkrainianRadioButton.Checked += OnCheckedChanged;

            _frequencyTable = new FrequencyTable();
            DataContext = this;
        }

        private async void OnCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (EnglishRadioButton.IsChecked == true)
                _alphabet = Alphabet.English;
            else if (UkrainianRadioButton.IsChecked == true) _alphabet = Alphabet.Ukrainian;
        }

        private async void GenreComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateFrequencies();
        }

        private async Task UpdateFrequencies()
        {
            if (_frequencyTable == null) return;
            _frequencyTable = new FrequencyTable();
            var genre = GenreComboBox.SelectedValue != null ? GenreComboBox.SelectedValue.ToString() : "";
            var url = GetSiteUrl(_alphabet.Code + genre.Replace("System.Windows.Controls.ComboBoxItem: ", ""));
            _frequencyTable.CalculateFrequencies(await DownloadText(url), _alphabet);

            DisplayFrequencies();
        }

        private void DisplayFrequencies()
        {
            var items = _frequencyTable.GetFrequencies()
                .Select(kvp =>
                    new FrequencyTableItem(kvp.Key.ToString(), Math.Round(kvp.Value, 6, MidpointRounding.AwayFromZero)))
                .ToList();
            DataGrid.ItemsSource = items;
        }

        private async Task<string> DownloadText(string url)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return await client.DownloadStringTaskAsync(url);
            }
        }

        private string GetSiteUrl(string siteName)
        {
            return ConfigurationManager.AppSettings[siteName];
        }

        private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                var fileContent = File.ReadAllText(fileName);
                _frequencyTable = new FrequencyTable();
                _frequencyTable.CalculateFrequencies(fileContent, _alphabet);
                DisplayFrequencies();
            }
        }
    }

    internal class FrequencyTableItem
    {
        public FrequencyTableItem(string letter, double frequency)
        {
            Letter = letter;
            Frequency = frequency;
        }

        public string Letter { get; }
        public double Frequency { get; }
    }
}