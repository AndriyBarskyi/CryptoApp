using System.Windows;
using CryptoApp.Ciphers.Alphabets;
using CryptoApp.Ciphers.CipherImpl;

namespace CryptoApp
{
    public partial class ActiveAttackWindow : Window
    {
        private readonly Alphabet _alphabet;
        public ActiveAttackWindow(string plainText, string encryptedText, Alphabet alphabet)
        {
            InitializeComponent();
            _alphabet = alphabet;
            MessageTextBox.Text = plainText;
            EncryptedMessageTextBox.Text = encryptedText;
        }

        private void FindKeyButton_OnClick(object sender, RoutedEventArgs e)
        {
            TrithemiusCipher trithemiusCipher = new TrithemiusCipher();
            ResultTextBox.Text = trithemiusCipher.AttackCipherKeys
                (EncryptedMessageTextBox.Text, MessageTextBox.Text, _alphabet);
        }
    }
}