using Microsoft.EntityFrameworkCore;
using PrepPal.Models;

namespace PrepPal.Contexts;

public class PrepPalDbContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    public PrepPalDbContext(DbContextOptions<PrepPalDbContext> options) : base(options)
    {
    }
}