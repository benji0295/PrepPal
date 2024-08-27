using PrepPal.Models;
using PrepPal.ViewModels;
using PrepPal.Views;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
    private RecipeViewModel _recipeViewModel;
    private GroceryListViewModel _groceryListViewModel;
    public Recipe SelectedRecipe { get; set; }

    public RecipeDetailPage(Recipe selectedRecipe, RecipeViewModel recipeViewModel, GroceryListViewModel groceryListViewModel)
    {
        InitializeComponent();
        SelectedRecipe = selectedRecipe;
        _recipeViewModel = recipeViewModel;
        _groceryListViewModel = groceryListViewModel;

        BindingContext = SelectedRecipe;
    }

    private async void OnDeleteRecipeClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete Recipe",
            $"Are you sure you want to delete your {SelectedRecipe.Name} recipe?", "Yes", "No");
        
        if (confirm)
        {
            _recipeViewModel.Recipes.Remove(SelectedRecipe);
            await DisplayAlert("Deleted", $"{SelectedRecipe.Name} recipe has been deleted.", "OK");
            await Navigation.PopAsync();
        }
    }

    private void OnAddIngredientsToGroceryListClicked(object sender, EventArgs e)
    {
        foreach (var ingredient in SelectedRecipe.Ingredients)
        {
            if (!_groceryListViewModel.GroceryItems.Any((item => item.Name == ingredient.Name)))
            {
                _groceryListViewModel.GroceryItems.Add(new GroceryItem { Name = ingredient.Name, IsBought = false });
            }
        }

        DisplayAlert("Success", "Ingredients added to your grocery list.", "OK");
    }
}
