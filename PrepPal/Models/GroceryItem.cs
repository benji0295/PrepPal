using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepPal.Models
{
    public class GroceryItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        
        private bool _isBought;

        public bool IsBought
        {
            get => _isBought;
            set
            {
                if (_isBought != value)
                {
                    _isBought = value;
                    OnPropertyChanged(nameof(IsBought));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
