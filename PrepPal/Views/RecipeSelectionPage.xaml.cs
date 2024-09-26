using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrepPal.Contexts;
using PrepPal.Models;
using PrepPal.ViewModels;

namespace PrepPal.Views;

[QueryProperty(nameof(Day), "day")]
public partial class RecipeSelectionPage : ContentPage
{
    public string Day { get; set; }
    private readonly PrepPalDbContext _dbContext;

    public RecipeSelectionPage()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<PrepPalDbContext>()
            .UseNpgsql("DataSource=app.db")
            .Options;

        _dbContext = new PrepPalDbContext(options);
        
        BindingContext = new RecipeViewModel(_dbContext);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        recipeCollectionView.SelectedItem = null;
    }
    
    public RecipeSelectionPage(PrepPalDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        BindingContext = new RecipeViewModel(_dbContext);
    }

    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;

        if (selectedRecipe != null)
        {
            (sender as CollectionView).SelectedItem = null;
            
            await Shell.Current.GoToAsync($"//MealPlanPage?recipe={selectedRecipe.Name}&day={Day}&image={selectedRecipe.ImageURL}");
        }
    }
}