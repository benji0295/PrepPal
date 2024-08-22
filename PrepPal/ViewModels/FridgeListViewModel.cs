using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    internal class FridgeListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FridgeItem> FridgeItems { get; set; }

        public FridgeListViewModel()
        {
            FridgeItems = new ObservableCollection<FridgeItem>()
            {
                new FridgeItem { Name = "Milk", ExpirationDate = DateTime.Now.AddDays(7), IsExpired = false, IsUsed = false },
                new FridgeItem { Name = "Eggs", ExpirationDate = DateTime.Now.AddDays(14), IsExpired = false, IsUsed = false },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
