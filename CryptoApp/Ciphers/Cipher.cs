using System;
using CryptoApp.Ciphers.Alphabets;

namespace CryptoApp.Ciphers
{
    public abstract class Cipher
    {
        public int Key { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        
        private FrequencyTable _frequencyTable;
    
        public FrequencyTable FrequencyTable 
        { 
            get => _frequencyTable;
            set 
            {
                _frequencyTable = value; 
                OnFrequencyTableChanged();
            }
        }
    
        public event EventHandler FrequencyTableChanged;
    
        protected virtual void OnFrequencyTableChanged()
        {
            FrequencyTableChanged?.Invoke(this, EventArgs.Empty);
        }
        public abstract string Encrypt(string input, Alphabet alphabet);
        public abstract string Decrypt(string input, Alphabet alphabet);

        public abstract string AttackCipher(string input, Alphabet alphabet);
    }
}