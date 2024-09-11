using Microsoft.EntityFrameworkCore;
using PrepPal.Models;

namespace PrepPal.Contexts;

public class PrepPalDbContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Aisle> Aisles { get; set; }
    public DbSet<StorageLocation> StorageLocations { get; set; }
    public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
    public DbSet<RecipeCategory> RecipeCategories { get; set; }

    public PrepPalDbContext(DbContextOptions<PrepPalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add any specific entity configurations or relationships here
        base.OnModelCreating(modelBuilder);
    }
}