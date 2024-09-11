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
                RecipeIngredients = ingredientsEditor.Text.Split('\n')
                    .Where(line => !string.IsNullOrWhiteSpace(line))  // Filter out empty lines
                    .Select(line => 
                    {
                        var parts = line.Split(' ');  // Assuming ingredients are input in "quantity unit name" format
                        return new RecipeIngredient
                        {
                            Quantity = decimal.Parse(parts[0]),  // Parse quantity
                            Unit = parts[1],                     // Unit (e.g., cups, tbsp, etc.)
                            IngredientName = string.Join(" ", parts.Skip(2))  // Ingredient name (after quantity and unit)
                        };
                    }).ToList(),
                Instructions = instructionsEditor.Text.Split('\n')
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select((line, index) => new Instruction 
                    { 
                        StepNumber = index + 1, 
                        Description = line.Trim() 
                    }).ToList()
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