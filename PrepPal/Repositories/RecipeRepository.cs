namespace PrepPal.Repositories;

public class RecipeRepository
{
    private readonly PrepPalDbContext _context;

    public RecipeRepository(PrepPalDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .Include(r => r.Instructions)
            .ToListAsync();
    }
    public async Task<List<Recipe>> GetFavoriteRecipesAsync()
    {
        return await _context.Recipes
            .Where(r => r.IsFavorite)
            .ToListAsync();
    }

    public async Task UpdateRecipeAsync(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync();
    }
    public async Task<List<FridgeItem>> GetFridgeItemsAsync()
    {
        return await _context.FridgeItems.ToListAsync();
    }

    public async Task AddBoughtItemsToFridgeAsync(List<GroceryItem> boughtItems)
    {
        foreach (var item in boughtItems)
        {
            var fridgeItem = new FridgeItem { Name = item.Name, LastBoughtDate = DateTime.Now };
            _context.FridgeItems.Add(fridgeItem);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<List<GroceryItem>> GetGroceryItemsAsync()
    {
        return await _context.GroceryItems.ToListAsync();
    }

    public async Task AddUsedItemsToGroceryAsync(List<FridgeItem> usedItems)
    {
        foreach (var item in usedItems)
        {
            var groceryItem = new GroceryItem { Name = item.Name, IsBought = false };
            _context.GroceryItems.Add(groceryItem);
        }
        await _context.SaveChangesAsync();
    }
}