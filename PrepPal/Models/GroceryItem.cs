using System;
using System.ComponentModel;

namespace PrepPal.Models
{
    public class GroceryItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string StorageLocation { get; set; }
        public string Aisle { get; set; }

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

        private bool _isInGroceryList;
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

        private DateTime _lastBoughtDate;
        public DateTime LastBoughtDate
        {
            get => _lastBoughtDate;
            set
            {
                if (_lastBoughtDate != value)
                {
                    _lastBoughtDate = value;
                    OnPropertyChanged(nameof(LastBoughtDate));
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