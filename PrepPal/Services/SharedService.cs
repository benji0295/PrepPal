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
                item.IsUsed = false;
                
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
                item.IsBought = false;
                
                var mainIngredientName = ExtractMainIngredient(item.Name);
                
                if (!FridgeItems.Any(i => i.Name == mainIngredientName))
                {
                    FridgeItems.Add(new FridgeItem
                    {
                        Name = mainIngredientName,
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
    }
}