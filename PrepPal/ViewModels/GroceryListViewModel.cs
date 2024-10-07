namespace PrepPal.ViewModels
{
    public class GroceryListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Grouping<string, GroceryItem>> GroupedGroceryItems { get; set; }

        private ObservableCollection<GroceryItem> _groceryItems;
        private readonly FridgeListViewModel _fridgeListViewModel;
        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddToFridgeCommand { get; }
        public ICommand ClearListCommand { get; }
        public ICommand DeleteSelectedCommand { get; }

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
                new GroceryItem
                    { Name = "Chicken Broth", IsBought = false, Aisle = "Canned Goods", StorageLocation = "Pantry" },
                new GroceryItem { Name = "Flour", IsBought = false, Aisle = "Baking", StorageLocation = "Pantry" },
                new GroceryItem
                    { Name = "Frozen Pizza", IsBought = false, Aisle = "Frozen", StorageLocation = "Freezer" },
                new GroceryItem { Name = "Oregano", IsBought = false, Aisle = "Spices", StorageLocation = "Pantry" }
            };

            DeleteItemCommand = new Command<GroceryItem>(DeleteItem);
            AddToFridgeCommand = new Command(AddGroceriesToFridge);
            ClearListCommand = new Command(ClearList);
            DeleteSelectedCommand = new Command(DeleteSelectedItems);

            GroupItems();
        }

        public void AddItemToGroceryList(string itemName)
        {
            var groceryItem =
                GroceryItems.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (groceryItem != null)
            {
                var fridgeItem = _fridgeListViewModel.FridgeItems.FirstOrDefault(i =>
                    i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (fridgeItem != null)
                {
                    groceryItem.IsInGroceryList = true;
                    groceryItem.LastBoughtDate = fridgeItem.LastBoughtDate; // Update the date
                }
            }
            else
            {
                var fridgeItem = _fridgeListViewModel.FridgeItems.FirstOrDefault(i =>
                    i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (fridgeItem != null)
                {
                    GroceryItems.Add(new GroceryItem
                    {
                        Name = itemName,
                        IsBought = false,
                        IsInGroceryList = true,
                        LastBoughtDate = fridgeItem.LastBoughtDate
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

        public void GroupItems()
        {
            if (GroceryItems == null || !GroceryItems.Any()) return;
            
            var grouped = GroceryItems
                .GroupBy(g => g.Aisle)
                .Select(g => new Grouping<string, GroceryItem>(g.Key, g.ToList()))
                .ToList();

            GroupedGroceryItems = new ObservableCollection<Grouping<string, GroceryItem>>(grouped);
            OnPropertyChanged(nameof(GroupedGroceryItems));
        }

        private void ClearList()
        {
            GroceryItems.Clear();
            GroupItems();
        }

        private void DeleteSelectedItems()
        {
            var itemsToRemove = GroceryItems.Where(item => item.IsBought).ToList();
            foreach (var item in itemsToRemove)
            {
                GroceryItems.Remove(item);
            }

            GroupItems();
        }

        private void DeleteItem(GroceryItem item)
        {
            if (item != null && GroceryItems.Contains(item))
            {
                GroceryItems.Remove(item);
            }

            GroupItems();
        }

        private void AddGroceriesToFridge()
        {
            var boughtItems = GroceryItems.Where(item => item.IsBought).ToList();

            if (boughtItems.Count == 0)
            {
                return;
            }

            foreach (var item in boughtItems)
            {
                var fridgeItem = _fridgeListViewModel.FridgeItems
                    .FirstOrDefault(f => f.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));

                if (fridgeItem == null)
                {
                    var mainIngredientName = ExtractMainIngredient(item.Name);

                    var newFridgeItem = new FridgeItem
                    {
                        Name = mainIngredientName,
                        LastBoughtDate = DateTime.Now,
                        IsUsed = false,
                        StorageLocation = item.StorageLocation ?? "Other"
                    };
                    _fridgeListViewModel.FridgeItems.Add(newFridgeItem);
                }
                else
                {
                    fridgeItem.LastBoughtDate = DateTime.Now;
                }
            }

            foreach (var item in boughtItems)
            {
                GroceryItems.Remove(item);
            }

            GroupItems();
            _fridgeListViewModel.GroupItems();

            OnPropertyChanged(nameof(GroceryItems));
            OnPropertyChanged(nameof(GroupedGroceryItems));

            _fridgeListViewModel.OnPropertyChanged(nameof(_fridgeListViewModel.FridgeItems));
            _fridgeListViewModel.OnPropertyChanged(nameof(_fridgeListViewModel.GroupedFridgeItems));
        }

        private string ExtractMainIngredient(string fullIngredient)
        {
            var descriptors = new List<string> { "to taste", "for garnish" };
            
            foreach (var descriptor in descriptors)
            {
                if (fullIngredient.Contains(descriptor))
                {
                    fullIngredient = fullIngredient.Replace(descriptor, "").Trim();
                }
            }
            
            var parts = fullIngredient.Split(' ');
            
            var startIndex = 0;

            for (int i = 0; i < parts.Length; i++)
            {
                if (!IsNumeric(parts[i]) && !IsCommonUnit(parts[i]))
                {
                    startIndex = i;
                    break;
                }
            }
            
            var mainIngredient = string.Join(" ", parts.Skip(startIndex));
            
            return mainIngredient.Contains(",") 
                ? mainIngredient.Split(',')[0].Trim() 
                : mainIngredient;
        }

        private bool IsNumeric(string value)
        {
            return decimal.TryParse(value, out _);
        }

        private bool IsCommonUnit(string value)
        {
            var units = new List<string> { "cup", "cups", "tsp", "tbsp", "oz", "g", "lb", "pound", "head" ,"teaspoon", "tablespoon", "ml", "liters", "grams", "kilogram", "kg", "pcs", "cloves" };
            return units.Contains(value.ToLower());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
