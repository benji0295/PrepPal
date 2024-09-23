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
    private string _selectedImagePath;
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

    private async void OnSelectImageButtonClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Select Image Source", "Cancel", null, "Take Photo", "Choose from Gallery", "Choose from Files");

        switch (action)
        {
            case "Take Photo":
                await TakePhotoAsync();
                break;
            case "Choose from Gallery":
                await PickPhotoFromGalleryAsync();
                break;
            case "Choose from Files":
                await PickPhotoFromFilesAsync();
                break;
        }
    }
    
    // Take a photo using the camera
    private async Task TakePhotoAsync()
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                _selectedImagePath = photo.FullPath;
                recipeImagePreview.Source = ImageSource.FromStream(() => stream);
                recipeImagePreview.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to take photo: {ex.Message}", "OK");
        }
    }
    
    // Pick an image from the gallery
    private async Task PickPhotoFromGalleryAsync()
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                _selectedImagePath = photo.FullPath;
                recipeImagePreview.Source = ImageSource.FromStream(() => stream);
                recipeImagePreview.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to pick photo: {ex.Message}", "OK");
        }
    }
    
    // Pick an image from the file system
    private async Task PickPhotoFromFilesAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick an image"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                _selectedImagePath = result.FullPath;
                recipeImagePreview.Source = ImageSource.FromStream(() => stream);
                recipeImagePreview.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to pick file: {ex.Message}", "OK");
        }
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        // Simply go back to the previous page without adding a recipe
        await Navigation.PopAsync();
    }
}