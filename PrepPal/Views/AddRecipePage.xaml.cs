using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrepPal.ViewModels;
using PrepPal.Models;

namespace PrepPal.Views;

public partial class AddRecipePage : ContentPage
{
    public AddRecipePage()
    {
        InitializeComponent();
    }

    async void OnAddRecipeClicked(object sender, EventArgs e)
    {
        try
        {
            // Create new Recipe instance
            Recipe newRecipe = new Recipe
            {
                Name = recipeNameEntry.Text,
                Category = categoryEntry.Text,
                Servings = int.Parse(servingsEntry.Text),
                PrepTime = int.Parse(prepTimeEntry.Text),
                CookTime = int.Parse(cookTimeEntry.Text),
                TotalTime = int.Parse(prepTimeEntry.Text + cookTimeEntry.Text),
                Source = sourceEntry.Text,
                SourceURL = sourceUrlEntry.Text,
                Ingredients = ingredientsEditor.Text.Split('\n').Select(line => new Ingredient { Name = line.Trim() }).ToList(),
                Instructions = instructionsEditor.Text.Split('\n').Select(line => new Instruction { Step = line.Trim() }).ToList()
            };

            // Add recipe to the RecipeViewModel's Recipes collection
            var viewModel = BindingContext as RecipeViewModel;
            viewModel?.Recipes.Add(newRecipe);

            // Navigate back to the RecipePage
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add recipe: {ex.Message}", "OK");
        }
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        // Simply go back to the previous page without adding a recipe
        await Navigation.PopAsync();
    }
}