using PrepPal.Models;
using PrepPal.ViewModels;

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
        InstructionsText = string.Join("\n", selectedRecipe.Instructions);
        IngredientsText = string.Join("\n", selectedRecipe.Ingredients);

        _groceryListViewModel = groceryListViewModel;

        BindingContext = this;
    }

    private void OnAddIngredientsToGroceryListClicked(object sender, EventArgs e)
    {
        foreach (var ingredient in SelectedRecipe.Ingredients)
        {
            // Debug
            DisplayAlert("Ingredient", ingredient, "OK");

            _groceryListViewModel.GroceryItems.Add(new GroceryItem { Name = ingredient, IsBought = false });
        }

        DisplayAlert("Success", "Ingredients added to grocery list", "OK");
    }
}
