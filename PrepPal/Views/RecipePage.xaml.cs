using PrepPal.ViewModels;
using PrepPal.Models;
using PrepPal.Views;
using PrepPal.Contexts;
using Microsoft.EntityFrameworkCore;

namespace PrepPal.Views;

public partial class RecipePage : ContentPage
{
	private GroceryListViewModel _groceryListViewModel;
	private RecipeViewModel _recipeViewModel;
	
	public RecipePage()
	{
		InitializeComponent();

		// Use the same PostgreSQL configuration
		var options = new DbContextOptionsBuilder<PrepPalDbContext>()
			.UseNpgsql("Host=localhost;Database=preppaldb")
			.Options;

		var dbContext = new PrepPalDbContext(options);
		BindingContext = new RecipeViewModel(dbContext); 
	}
    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        // Get the selected recipe
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;

        // If a recipe is selected, navigate to the RecipeDetailPage
        if (selectedRecipe != null)
        {
	        await Navigation.PushAsync(new RecipeDetailPage(selectedRecipe, BindingContext as RecipeViewModel, App.GroceryListViewModel));
	        ((CollectionView)sender).SelectedItem = null;
        }
    }

    async void OnAddNewRecipeClicked(object sender, EventArgs e)
    {
	    await Navigation.PushAsync(new AddRecipePage { BindingContext = BindingContext });
    }
}