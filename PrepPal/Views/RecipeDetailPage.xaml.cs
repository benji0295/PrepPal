using PrepPal.Models;

namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
    public Recipe SelectedRecipe { get; set; }
    public string InstructionsText { get; set; }
    public string IngredientsText { get; set; }

    public RecipeDetailPage(Recipe selectedRecipe)
    {
        InitializeComponent();

        SelectedRecipe = selectedRecipe;
        InstructionsText = string.Join("\n", selectedRecipe.Instructions);
        IngredientsText = string.Join("\n", selectedRecipe.Ingredients);

        BindingContext = this;
    }
}
