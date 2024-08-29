using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    public class FridgeListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FridgeItem> _fridgeItems;

        public ObservableCollection<FridgeItem> FridgeItems
        {
            get => _fridgeItems;
            set
            {
                _fridgeItems = value;
                OnPropertyChanged(nameof(FridgeItems));
            }
        }
        public ICommand DeleteItemCommand { get; set; }

        public FridgeListViewModel()
        {
            FridgeItems = new ObservableCollection<FridgeItem>()
            {
                new FridgeItem { Name = "Milk", ExpirationDate = DateTime.Now.AddDays(7), IsExpired = false, IsUsed = false },
                new FridgeItem { Name = "Eggs", ExpirationDate = DateTime.Now.AddDays(14), IsExpired = false, IsUsed = false },
            };

            DeleteItemCommand = new Command<FridgeItem>(DeleteItem);
        }
        
        private void DeleteItem(FridgeItem item)
        {
            if (item != null && FridgeItems.Contains(item))
            {
                FridgeItems.Remove(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
