using PrepPal.ViewModels;
using PrepPal.Models;
using PrepPal.Views;

namespace PrepPal.Views;

public partial class RecipePage : ContentPage
{
	private GroceryListViewModel _groceryListViewModel;
	private RecipeViewModel _recipeViewModel;
	
	public RecipePage()
	{
		InitializeComponent();
        _groceryListViewModel = new GroceryListViewModel();
        _recipeViewModel = new RecipeViewModel();
		BindingContext = _recipeViewModel;
	}
    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        // Get the selected recipe
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;

        // If a recipe is selected, navigate to the RecipeDetailPage
        if (selectedRecipe != null)
        {
	        await Navigation.PushAsync(new RecipeDetailPage(selectedRecipe, _groceryListViewModel));
	        ((CollectionView)sender).SelectedItem = null;
        }
    }
}