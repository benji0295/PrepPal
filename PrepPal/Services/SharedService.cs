namespace PrepPal.Services
{
    public class SharedService
    {
        public ObservableCollection<GroceryItem> GroceryItems { get; set; }
        public ObservableCollection<FridgeItem> FridgeItems { get; set; }
        
        public event Action OnFridgeItemsChanged;
        public event Action OnGroceryItemsChanged;

        public SharedService()
        {
            GroceryItems = new ObservableCollection<GroceryItem>();
            FridgeItems = new ObservableCollection<FridgeItem>();
        }

        public void AddUsedItemsToGroceryList(List<FridgeItem> usedItems)
        {
            foreach (var item in usedItems)
            {
                if (!GroceryItems.Any(i => i.Name == item.Name))
                {
                    GroceryItems.Add(new GroceryItem
                    {
                        Name = item.Name,
                        IsBought = false,
                        Aisle = item.Aisle,
                        StorageLocation = item.StorageLocation
                    });
                }
            }

            // Remove used items from fridge
            foreach (var item in usedItems)
            {
                FridgeItems.Remove(item);
            }
            
            OnGroceryItemsChanged?.Invoke();
            OnFridgeItemsChanged?.Invoke();
        }

        public void AddBoughtItemsToFridge(List<GroceryItem> boughtItems)
        {
            foreach (var item in boughtItems)
            {
                if (!FridgeItems.Any(i => i.Name == item.Name))
                {
                    FridgeItems.Add(new FridgeItem
                    {
                        Name = item.Name,
                        LastBoughtDate = DateTime.Now,
                        IsUsed = false,
                        StorageLocation = item.StorageLocation
                    });
                }
            }

            // Remove bought items from grocery list
            foreach (var item in boughtItems)
            {
                GroceryItems.Remove(item);
            }
            
            OnFridgeItemsChanged?.Invoke();
            OnGroceryItemsChanged?.Invoke();
        }
    }
}