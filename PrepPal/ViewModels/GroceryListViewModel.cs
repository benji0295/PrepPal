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
    public class GroceryListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<GroceryItem> GroceryItems { get; set; }

        public GroceryListViewModel()
        {
            GroceryItems = new ObservableCollection<GroceryItem>()
            {
                new GroceryItem { Name = "Milk", IsBought = false },
                new GroceryItem { Name = "Eggs", IsBought = false },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
