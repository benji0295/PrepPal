using System.Windows.Input;
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
        AdjustImageWidth();

        BindingContext = SelectedRecipe;

        UpdateFavoriteIcon();
    }

    private void AdjustImageWidth()
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            RecipeImage.WidthRequest = Application.Current.MainPage.Width;
        }
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
        foreach (var ingredient in SelectedRecipe.RecipeIngredients)
        {
            if (!_groceryListViewModel.GroceryItems.Any((item => item.Name == ingredient.IngredientName)))
            {
                _groceryListViewModel.GroceryItems.Add(new GroceryItem { Name = ingredient.IngredientName, IsBought = false });
            }
        }

        DisplayAlert("Success", "Ingredients added to your grocery list.", "OK");
    }

    private void OnFavoriteClicked(object sender, EventArgs e)
    {
        SelectedRecipe.IsFavorite = !SelectedRecipe.IsFavorite;

        if (SelectedRecipe.IsFavorite)
        {
            // Add to favorites
            if (!App.FavoriteRecipesViewModel.FavoriteRecipes.Contains(SelectedRecipe))
            {
                FavoriteToolbarItem.IconImageSource = "heart_icon_filled.png";
                App.FavoriteRecipesViewModel.FavoriteRecipes.Add(SelectedRecipe);
            }
        }
        else
        {
            // Remove from favorites
            if (App.FavoriteRecipesViewModel.FavoriteRecipes.Contains(SelectedRecipe))
            {
                FavoriteToolbarItem.IconImageSource = "heart_icon.png";
                App.FavoriteRecipesViewModel.FavoriteRecipes.Remove(SelectedRecipe);
            }
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFavoriteIcon();

        foreach (var ingredient in SelectedRecipe.RecipeIngredients)
        {
            ingredient.IsSelected = false;
        }

        foreach (var instruction in SelectedRecipe.Instructions)
        {
            instruction.IsCompleted = false;
        }
    }

    private void UpdateFavoriteIcon()
    {
        FavoriteToolbarItem.IconImageSource = SelectedRecipe.IsFavorite ? "heart_icon_filled.png" : "heart_icon.png";
    }
}
