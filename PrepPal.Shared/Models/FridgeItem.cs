using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepPal.Models
{
    public class FridgeItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string StorageLocation { get; set; }
        public string Aisle { get; set; }
        public DateTime LastBoughtDate { get; set; }
        public bool IsExpired { get; set; }
        private bool _isUsed;
        private bool _isInGroceryList = false;

        public bool IsInGroceryList
        {
            get => _isInGroceryList;
            set
            {
                if (_isInGroceryList != value)
                {
                    _isInGroceryList = value;
                    OnPropertyChanged(nameof(IsInGroceryList));
                }
            }
        }
        
        public bool IsUsed
        {
            get => _isUsed;
            set
            {
                if (_isUsed != value)
                {
                    _isUsed = value;
                    OnPropertyChanged(nameof(IsUsed));
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
