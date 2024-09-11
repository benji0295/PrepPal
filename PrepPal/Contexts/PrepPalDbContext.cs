using Microsoft.EntityFrameworkCore;
using PrepPal.Models;

namespace PrepPal.Contexts;
public class PrepPalDbContext : DbContext
{
    public PrepPalDbContext(DbContextOptions<PrepPalDbContext> options) : base(options)
    {
    }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<Aisle> Aisles { get; set; }
    public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
    public DbSet<StorageLocation> StorageLocations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customize the model, e.g., relationships, constraints
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany()
            .HasForeignKey(ri => ri.IngredientId);

        modelBuilder.Entity<Instruction>()
            .HasOne(i => i.Recipe)
            .WithMany(r => r.Instructions)
            .HasForeignKey(i => i.RecipeId);
    }
}