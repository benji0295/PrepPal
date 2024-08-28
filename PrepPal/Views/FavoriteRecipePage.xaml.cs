using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrepPal.ViewModels;
using PrepPal.Models;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

public partial class FavoriteRecipePage : ContentPage
{
    public FavoriteRecipePage()
    {
        InitializeComponent();
        BindingContext = App.FavoriteRecipesViewModel;
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
}