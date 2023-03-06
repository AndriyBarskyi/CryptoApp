using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CryptoApp.Ciphers.Alphabets;
using Cryptologist.Ciphers;
using Cryptologist.Ciphers.Utils;
using Microsoft.Win32;

namespace CryptoApp
{
    public partial class FrequencyTableWindow : Window
    {
        private FrequencyTable _frequencyTable;
        private Alphabet _alphabet = Alphabet.English;

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
            {
                _alphabet = Alphabet.English;
            }
            else if (UkrainianRadioButton.IsChecked == true)
            {
                _alphabet = Alphabet.Ukrainian;
            }
        }

        private async void GenreComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateFrequencies();
        }

        private async Task UpdateFrequencies()
        {
            if (_frequencyTable == null)
            {
                return;
            }
            _frequencyTable = new FrequencyTable();
            string genre = GenreComboBox.SelectedValue != null ? GenreComboBox.SelectedValue.ToString() : "";
            string url = GetSiteUrl(_alphabet.Code + genre.Replace("System.Windows.Controls.ComboBoxItem: ", ""));
            _frequencyTable.CalculateFrequencies(await DownloadText(url), _alphabet);
            
            DisplayFrequencies();
        }

        private void DisplayFrequencies()
        {
            List<FrequencyTableItem> items = _frequencyTable.GetFrequencies()
                .Select(kvp =>
                    new FrequencyTableItem(kvp.Key.ToString(), Math.Round(kvp.Value, digits: 6, MidpointRounding.AwayFromZero)))
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string fileContent = File.ReadAllText(fileName);
                _frequencyTable = new FrequencyTable();
                _frequencyTable.CalculateFrequencies(fileContent, _alphabet);
                DisplayFrequencies();
            }
        }
    }

    class FrequencyTableItem
    {
        public string Letter { get;}
        public double Frequency { get; }

        public FrequencyTableItem(string letter, double frequency)
        {
            Letter = letter;
            Frequency = frequency;
        }
    }
}