using PrepPal.Models;
using PrepPal.ViewModels;
using PrepPal.Views;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
    private GroceryListViewModel _groceryListViewModel;
    public Recipe SelectedRecipe { get; set; }

    public RecipeDetailPage(Recipe selectedRecipe, GroceryListViewModel groceryListViewModel)
    {
        InitializeComponent();

        SelectedRecipe = selectedRecipe;
        _groceryListViewModel = groceryListViewModel;

        BindingContext = SelectedRecipe;
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
