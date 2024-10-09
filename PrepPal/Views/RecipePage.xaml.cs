using PrepPal.ViewModels;
using PrepPal.Models;
using PrepPal.Views;
using PrepPal.Data;
using Microsoft.EntityFrameworkCore;
using PrepPal.Repositories;

namespace PrepPal.Views;

public partial class RecipePage : ContentPage
{
	private GroceryListViewModel _groceryListViewModel;
	private RecipeRepository _recipeRepository;
	
	public RecipePage(RecipeViewModel viewModel)
	{
		InitializeComponent();
		
		BindingContext = viewModel; 
	}
    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;
        
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