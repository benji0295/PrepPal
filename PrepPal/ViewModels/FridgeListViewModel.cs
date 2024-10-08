using PrepPal.Services;

namespace PrepPal.ViewModels
{
    public enum FilterType
    {
        ByStorageLocation,
        ByDateBought
    }
    
    public class FridgeListViewModel : INotifyPropertyChanged
    {
        private SharedService _sharedService;
        
        public ObservableCollection<FridgeItem> FridgeItems => _sharedService.FridgeItems;
        public ObservableCollection<Grouping<string, FridgeItem>> GroupedFridgeItems { get; set; }

        public FilterType CurrentFilter { get; set; } = FilterType.ByStorageLocation;
        public ICommand DeleteItemCommand { get; set; }
        public ICommand ClearListCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
        public ICommand AddUsedToGroceryCommand { get; }

        public FridgeListViewModel(SharedService sharedService)
        {
            _sharedService = sharedService;
            
            _sharedService.OnFridgeItemsChanged += () =>
            {
                OnPropertyChanged(nameof(FridgeItems));
                GroupItems();
            };
            
            _sharedService.FridgeItems.Add(new FridgeItem
            {
                Name = "Milk", LastBoughtDate = DateTime.Now.AddDays(-3), IsUsed = false, Aisle = "Dairy",
                StorageLocation = "Fridge"
            });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Eggs", LastBoughtDate = DateTime.Now.AddDays(-10), IsUsed = false, Aisle = "Dairy",
                    StorageLocation = "Fridge"
                });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Apples", LastBoughtDate = DateTime.Now, IsUsed = false, Aisle = "Produce",
                    StorageLocation = "Fridge"
                });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Ice Cream", LastBoughtDate = DateTime.Now.AddDays(-14), IsUsed = false, Aisle = "Frozen",
                    StorageLocation = "Freezer"
                });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Ketchup", LastBoughtDate = DateTime.Now.AddDays(-20), IsUsed = false, Aisle = "Condiments",
                    StorageLocation = "Fridge"
                });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Frozen Peas", LastBoughtDate = DateTime.Now.AddDays(-5), IsUsed = false, Aisle = "Frozen",
                    StorageLocation = "Freezer"
                });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Spaghetti", LastBoughtDate = DateTime.Now.AddDays(-5), IsUsed = false, Aisle = "Pasta",
                    StorageLocation = "Pantry"
                });
            _sharedService.FridgeItems.Add(new FridgeItem
                {
                    Name = "Peanut Butter", LastBoughtDate = DateTime.Now.AddDays(-37), IsUsed = false,
                    Aisle = "Condiments", StorageLocation = "Pantry"
                });

            DeleteItemCommand = new Command<FridgeItem>(DeleteItem);
            ClearListCommand = new Command(ClearList);
            DeleteSelectedCommand = new Command(DeleteSelectedItems);
            AddUsedToGroceryCommand = new Command(AddUsedItemsToGroceryList);
            
            GroupItems();
        }
        private void AddUsedItemsToGroceryList()
        {
            var usedItems = FridgeItems.Where(item => item.IsUsed).ToList();

            if (usedItems.Any())
            {
                _sharedService.AddUsedItemsToGroceryList(usedItems);
                
                // Group items again after removal
                GroupItems();
                
                // Notify the UI of changes
                OnPropertyChanged(nameof(FridgeItems));
                OnPropertyChanged(nameof(GroupedFridgeItems));
            }
        }



        public void ApplyFilter()
        {
            switch (CurrentFilter)
            {
                case FilterType.ByStorageLocation:
                    Console.WriteLine("Grouping by Storage Location.");
                    GroupByStorageLocation();
                    break;
                case FilterType.ByDateBought:
                    Console.WriteLine("Grouping by Date Bought.");
                    GroupByDateBought();
                    break;
            }
            Console.WriteLine("Filter applied. Triggering UI update.");
            OnPropertyChanged(nameof(GroupedFridgeItems));
        }

        private void GroupByStorageLocation()
        {
            Console.WriteLine("Grouping by Storage Location.");
            var grouped = FridgeItems
                .GroupBy(f => f.StorageLocation)
                .Select(g => new Grouping<string, FridgeItem>(g.Key, g.ToList()))
                .ToList();
            
            Console.WriteLine($"Grouped by Storage Location. Groups: {grouped.Count}");
            foreach (var group in grouped)
            {
                Console.WriteLine($"Group: {group.Key}, Items: {group.Count()}");
            }

            GroupedFridgeItems = new ObservableCollection<Grouping<string, FridgeItem>>(grouped);
            
            Console.WriteLine("GroupedFridgeItems updated. Notifying UI.");
            OnPropertyChanged(nameof(GroupedFridgeItems));
        }
        public void GroupByDateBought()
        {
            Console.WriteLine("Grouping by Date Bought.");
            
            var grouped = FridgeItems
                .GroupBy(f => GetDateRange(f.LastBoughtDate))
                .Select(g => new Grouping<string, FridgeItem>(
                    g.Key, 
                    g.OrderByDescending(f => f.LastBoughtDate).ToList()))
                .ToList();
            
            Console.WriteLine($"Grouped by Date Bought. Groups: {grouped.Count}");
            foreach (var group in grouped)
            {
                Console.WriteLine($"Group: {group.Key}, Items: {group.Count()}");
            }

            GroupedFridgeItems = new ObservableCollection<Grouping<string, FridgeItem>>(grouped);
            
            Console.WriteLine("GroupedFridgeItems updated. Notifying UI.");
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
