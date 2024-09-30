using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PrepPal.Models;
using PrepPal.ViewModels;

namespace PrepPal.ViewModels
{
    public class GroceryListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Grouping<string, GroceryItem>> GroupedGroceryItems { get; set; }
        
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
                GroupItems();
            }
        }


        public GroceryListViewModel(FridgeListViewModel fridgeListViewModel)
        {
            _fridgeListViewModel = fridgeListViewModel;
            
            GroceryItems = new ObservableCollection<GroceryItem>()
            {
                new GroceryItem { Name = "Apples", IsBought = false, Aisle = "Produce", StorageLocation = "Fridge" },
                new GroceryItem { Name = "Bananas", IsBought = false, Aisle = "Produce", StorageLocation = "Counter" },
                new GroceryItem { Name = "Carrots", IsBought = false, Aisle = "Produce", StorageLocation = "Fridge" },
                new GroceryItem { Name = "Chicken Broth", IsBought = false, Aisle = "Canned Goods", StorageLocation = "Pantry"},
                new GroceryItem { Name = "Flour", IsBought = false, Aisle = "Baking", StorageLocation = "Pantry"},
                new GroceryItem { Name = "Frozen Pizza", IsBought = false, Aisle = "Frozen", StorageLocation = "Freezer"},
                new GroceryItem { Name = "Oregano", IsBought = false, Aisle = "Spices", StorageLocation = "Pantry"}
            };

            DeleteItemCommand = new Command<GroceryItem>(DeleteItem);
            AddToFridgeCommand = new Command(AddGroceriesToFridge);
            
            GroupItems();
        }
        public void AddItemToGroceryList(string itemName)
        {
            // Check if the item is already in the grocery list
            var groceryItem = GroceryItems.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (groceryItem != null)
            {
                // If item exists in the grocery list, just update the IsInGroceryList property
                var fridgeItem = _fridgeListViewModel.FridgeItems.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (fridgeItem != null)
                {
                    groceryItem.IsInGroceryList = true;
                    groceryItem.LastBoughtDate = fridgeItem.LastBoughtDate; // Update the date
                }
            }
            else
            {
                // If item doesn't exist in the grocery list, create a new one
                var fridgeItem = _fridgeListViewModel.FridgeItems.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (fridgeItem != null)
                {
                    GroceryItems.Add(new GroceryItem
                    {
                        Name = itemName,
                        IsBought = false,
                        IsInGroceryList = true,
                        LastBoughtDate = fridgeItem.LastBoughtDate // Set the date
                    });
                }
                else
                {
                    GroceryItems.Add(new GroceryItem
                    {
                        Name = itemName,
                        IsBought = false,
                        IsInGroceryList = false
                    });
                }
            }
        }
        private void GroupItems()
        {
            var grouped = GroceryItems
                .GroupBy(g => g.Aisle) // Group by Aisle
                .Select(g => new Grouping<string, GroceryItem>(g.Key, g.ToList()))
                .ToList();

            GroupedGroceryItems = new ObservableCollection<Grouping<string, GroceryItem>>(grouped);
            OnPropertyChanged(nameof(GroupedGroceryItems));
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
                var fridgeItem = _fridgeListViewModel.FridgeItems.FirstOrDefault(f => f.Name == item.Name);

                if (fridgeItem == null)
                {
                    _fridgeListViewModel.FridgeItems.Add(new FridgeItem { Name = item.Name, LastBoughtDate = DateTime.Now, IsUsed = false });
                }
                else
                {
                    // Update the LastBoughtDate if the item is already in the fridge
                    fridgeItem.LastBoughtDate = DateTime.Now;
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
