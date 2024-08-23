using PrepPal.Models;
using PrepPal.ViewModels;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
    private GroceryListViewModel _groceryListViewModel;
    
    public Recipe SelectedRecipe { get; set; }
    public string InstructionsText { get; set; }
    public string IngredientsText { get; set; }

    public RecipeDetailPage(Recipe selectedRecipe, GroceryListViewModel groceryListViewModel)
    {
        InitializeComponent();

        SelectedRecipe = selectedRecipe;
        _groceryListViewModel = groceryListViewModel;
        
        InstructionsText = string.Join("\n", selectedRecipe.Instructions);
        IngredientsText = string.Join("\n", selectedRecipe.Ingredients);

        _groceryListViewModel = groceryListViewModel;

        BindingContext = this;
    }

    private void OnAddToGroceryListClicked(object sender, EventArgs e)
    {
        if (SelectedRecipe == null)
        {
            DisplayAlert("Error", "No recipe selected.", "OK");
            return;
        }

        if (SelectedRecipe.Ingredients == null || !SelectedRecipe.Ingredients.Any())
        {
            DisplayAlert("Error", "No ingredients found.", "OK");
            return;
        }

        if (_groceryListViewModel == null)
        {
            DisplayAlert("Error", "Grocery list view model not initialized.", "OK");
            return;
        }

        foreach (var ingredient in SelectedRecipe.Ingredients)
        {
            if (!_groceryListViewModel.GroceryItems.Any(item => item.Name == ingredient))
            {
                _groceryListViewModel.GroceryItems.Add(new GroceryItem { Name = ingredient, IsBought = false});
            }
        }

        DisplayAlert("Success", "Ingredients added to grocery list.", "OK");
    }
}
