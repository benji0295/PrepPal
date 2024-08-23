using PrepPal.Models;
using PrepPal.ViewModels;
using PrepPal.Views;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
    
    public Recipe SelectedRecipe { get; set; }

    public RecipeDetailPage(Recipe selectedRecipe)
    {
        InitializeComponent();

        SelectedRecipe = selectedRecipe;

        BindingContext = SelectedRecipe;
    }
}
