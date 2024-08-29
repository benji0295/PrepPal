using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    public class GroceryListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GroceryItem> _groceryItems;
        private readonly FridgeListViewModel _fridgeListViewModel;
        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddToFridgeCommand { get; }

        public ObservableCollection<GroceryItem> GroceryItems
        {
            get => _groceryItems;
            set
            {
                _groceryItems = value;
                OnPropertyChanged(nameof(GroceryItems));
            }
        }


        public GroceryListViewModel(FridgeListViewModel fridgeListViewModel)
        {
            _fridgeListViewModel = fridgeListViewModel;
            
            GroceryItems = new ObservableCollection<GroceryItem>()
            {
                new GroceryItem { Name = "Apples", IsBought = false },
                new GroceryItem { Name = "Bananas", IsBought = false },
                new GroceryItem { Name = "Carrots", IsBought = false },
            };

            DeleteItemCommand = new Command<GroceryItem>(DeleteItem);
            AddToFridgeCommand = new Command(AddGroceriesToFridge);
        }

        private void DeleteItem(GroceryItem item)
        {
            if (item != null && GroceryItems.Contains(item))
            {
                GroceryItems.Remove(item);
            }
        }

        private void AddGroceriesToFridge()
        {
            var boughtItems = GroceryItems.Where(item => item.IsBought).ToList();

            foreach (var item in boughtItems)
            {
                // Adding the bought grocery items to the fridge list
                if (!_fridgeListViewModel.FridgeItems.Any(f => f.Name == item.Name))
                {
                    _fridgeListViewModel.FridgeItems.Add(new FridgeItem { Name = item.Name, LastBoughtDate = DateTime.Now, IsUsed = false });
                }

                // Remove the item from the grocery list
                GroceryItems.Remove(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
