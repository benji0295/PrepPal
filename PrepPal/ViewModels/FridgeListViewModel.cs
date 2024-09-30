using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks.Dataflow;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    public class FridgeListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FridgeItem> _fridgeItems;
        public ObservableCollection<Grouping<string, FridgeItem>> GroupedFridgeItems { get; set; }

        public ObservableCollection<FridgeItem> FridgeItems
        {
            get => _fridgeItems;
            set
            {
                _fridgeItems = value;
                OnPropertyChanged(nameof(FridgeItems));
                GroupItems();
            }
        }
        public ICommand DeleteItemCommand { get; set; }

        public FridgeListViewModel()
        {
            FridgeItems = new ObservableCollection<FridgeItem>()
            {
                new FridgeItem { Name = "Milk", LastBoughtDate = DateTime.Now.AddDays(-3), IsUsed = false, Aisle = "Dairy", StorageLocation = "Fridge"},
                new FridgeItem { Name = "Eggs", LastBoughtDate = DateTime.Now.AddDays(-10), IsUsed = false, Aisle = "Dairy", StorageLocation = "Fridge"},
                new FridgeItem { Name = "Apples", LastBoughtDate = DateTime.Now, IsUsed = false, Aisle = "Produce", StorageLocation = "Fridge"},
                new FridgeItem { Name = "Ice Cream", LastBoughtDate = DateTime.Now.AddDays(-14), IsUsed = false, Aisle = "Frozen", StorageLocation = "Freezer"},
                new FridgeItem { Name = "Ketchup", LastBoughtDate = DateTime.Now.AddDays(-20), IsUsed = false, Aisle = "Condiments", StorageLocation = "Fridge"},
                new FridgeItem { Name = "Frozen Peas", LastBoughtDate = DateTime.Now.AddDays(-5), IsUsed = false, Aisle = "Frozen", StorageLocation = "Freezer"},
                new FridgeItem { Name = "Spaghetti", LastBoughtDate = DateTime.Now.AddDays(-5), IsUsed = false, Aisle = "Pasta", StorageLocation = "Pantry"},
                new FridgeItem { Name = "Peanut Butter", LastBoughtDate = DateTime.Now.AddDays(-3), IsUsed = false, Aisle = "Condiments", StorageLocation = "Pantry"},
            };

            DeleteItemCommand = new Command<FridgeItem>(DeleteItem);
            GroupItems();
        }

        private void GroupItems()
        {
            var grouped = FridgeItems
                .GroupBy(f => f.StorageLocation) // Group by storage location
                .Select(g => new Grouping<string, FridgeItem>(g.Key, g.ToList()))
                .ToList();

            GroupedFridgeItems = new ObservableCollection<Grouping<string, FridgeItem>>(grouped);
            OnPropertyChanged(nameof(GroupedFridgeItems));
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
