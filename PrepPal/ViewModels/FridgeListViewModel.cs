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
    public enum FilterType
    {
        ByStorageLocation,
        ByDateBought
    }
    
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

        public FilterType CurrentFilter { get; set; } = FilterType.ByStorageLocation;
        public ICommand DeleteItemCommand { get; set; }
        public ICommand ClearListCommand { get; }
        public ICommand DeleteSelectedCommand { get; }

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
                new FridgeItem { Name = "Peanut Butter", LastBoughtDate = DateTime.Now.AddDays(-37), IsUsed = false, Aisle = "Condiments", StorageLocation = "Pantry"},
            };

            DeleteItemCommand = new Command<FridgeItem>(DeleteItem);
            ClearListCommand = new Command(ClearList);
            DeleteSelectedCommand = new Command(DeleteSelectedItems);
            GroupItems();
        }

        public void ApplyFilter()
        {
            switch (CurrentFilter)
            {
                case FilterType.ByStorageLocation:
                    GroupByStorageLocation();
                    break;
                case FilterType.ByDateBought:
                    GroupByDateBought();
                    break;
            }
        }

        private void GroupByStorageLocation()
        {
            var grouped = FridgeItems
                .GroupBy(f => f.StorageLocation)
                .Select(g => new Grouping<string, FridgeItem>(g.Key, g.ToList()))
                .ToList();

            GroupedFridgeItems = new ObservableCollection<Grouping<string, FridgeItem>>(grouped);
            OnPropertyChanged(nameof(GroupedFridgeItems));
        }
        private void GroupByDateBought()
        {
            var grouped = FridgeItems
                .GroupBy(f => GetDateRange(f.LastBoughtDate))
                .Select(g => new Grouping<string, FridgeItem>(
                    g.Key, 
                    g.OrderByDescending(f => f.LastBoughtDate).ToList()))
                .ToList();

            GroupedFridgeItems = new ObservableCollection<Grouping<string, FridgeItem>>(grouped);
            OnPropertyChanged(nameof(GroupedFridgeItems));
        }

        private string GetDateRange(DateTime date)
        {
            var daysAgo = (DateTime.Now - date).TotalDays;

            if (daysAgo <= 14)
            {
                return "Bought within last 2 weeks";
            }
            else if (daysAgo <= 30)
            {
                return "Bought within last month";
            }
            else
            {
                return "Bought over a month ago";
            }
        }

        public void GroupItems()
        {
            var grouped = FridgeItems
                .GroupBy(f => f.StorageLocation) 
                .Select(g => new Grouping<string, FridgeItem>(g.Key, g.ToList()))
                .ToList();

            GroupedFridgeItems = new ObservableCollection<Grouping<string, FridgeItem>>(grouped);
            OnPropertyChanged(nameof(GroupedFridgeItems));
        }

        private void ClearList()
        {
            FridgeItems.Clear();
            GroupItems();
        }

        private void DeleteSelectedItems()
        {
            var itemsToRemove = FridgeItems.Where(item => item.IsUsed).ToList();

            foreach (var item in itemsToRemove)
            {
                FridgeItems.Remove(item);
            }
            GroupItems();
        }
        
        private void DeleteItem(FridgeItem item)
        {
            if (item != null && FridgeItems.Contains(item))
            {
                FridgeItems.Remove(item);
            }
            GroupItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
