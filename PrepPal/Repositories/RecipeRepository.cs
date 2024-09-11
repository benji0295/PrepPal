using PrepPal.Contexts;
using PrepPal.Models;
using Microsoft.EntityFrameworkCore;

namespace PrepPal.Repositories;

public class RecipeRepository
{
    private readonly PrepPalDbContext _context;

    public RecipeRepository(PrepPalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Recipe>> GetAllRecipes()
    {
        return await _context.Recipes.ToListAsync();
    }

    public async Task AddRecipe(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
    }
}