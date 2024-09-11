using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrepPal.Contexts;
using PrepPal.ViewModels;
using PrepPal.Models;

namespace PrepPal.Views;

public partial class AddRecipePage : ContentPage
{
    private readonly PrepPalDbContext _context;
    public AddRecipePage(PrepPalDbContext context)
    {
        InitializeComponent();
        _context = context;
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
                SourceURL = sourceUrlEntry.Text
            };
            var ingredientLines = ingredientsEditor.Text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
            foreach (var line in ingredientLines)
            {
                var parts = line.Split(' ');
                if (parts.Length >= 3)
                {
                    var quantity = decimal.Parse(parts[0]);
                    var unit = parts[1];
                    var ingredientName = string.Join(" ", parts.Skip(2));

                    // Assume the ingredient already exists in the database or create a new one.
                    var ingredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredientName) ??
                                     new Ingredient { Name = ingredientName };

                    var recipeIngredient = new RecipeIngredient
                    {
                        Ingredient = ingredient,
                        Quantity = quantity,
                        Unit = unit,
                        IngredientName = ingredientName
                    };

                    newRecipe.Ingredients.Add(recipeIngredient);
                }
            }

            var instructionLines = instructionsEditor.Text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
            for (int i = 0; i < instructionLines.Count; i++)
            {
                var instruction = new Instruction
                {
                    Step = i + 1,
                    Description = instructionLines[i].Trim()
                };

                newRecipe.Instructions.Add(instruction);
            }

            _context.Recipes.Add(newRecipe);
            await _context.SaveChangesAsync();

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