using PrepPal.ViewModels;
using PrepPal.Models;
using PrepPal.Views;

namespace PrepPal.Views;

public partial class RecipePage : ContentPage
{
	private GroceryListViewModel _groceryListViewModel;
	
	public RecipePage()
	{
		InitializeComponent();

		_groceryListViewModel = new GroceryListViewModel();
		BindingContext = new RecipeViewModel();
	}
    private void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        // Get the selected recipe
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;

        // If a recipe is selected, navigate to the RecipeDetailPage
        if (selectedRecipe != null)
        {
            // Clear the selection (optional)
            ((CollectionView)sender).SelectedItem = null;

            // Navigate to the RecipeDetailPage with the selected recipe
            Navigation.PushAsync(new RecipeDetailPage(selectedRecipe, _groceryListViewModel));
        }
    }
}