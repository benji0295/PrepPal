using PrepPal.Contexts;
using PrepPal.ViewModels;
using PrepPal.Models;
using PrepPal.Views;
using PrepPal.Contexts;

namespace PrepPal.Views;

public partial class RecipePage : ContentPage
{
	private GroceryListViewModel _groceryListViewModel;
	private RecipeViewModel _recipeViewModel;
	
	public RecipePage(PrepPalDbContext context)
	{
		InitializeComponent();
		
        _groceryListViewModel = new GroceryListViewModel(App.FridgeListViewModel);
        _recipeViewModel = new RecipeViewModel(context);
        
		BindingContext = _recipeViewModel;
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

    async void OnAddNewRecipeClicked(object sender, EventArgs e)
    {
	    var addRecipePage = new AddRecipePage(_recipeViewModel.DbContext);
	    addRecipePage.BindingContext = _recipeViewModel;
    
	    await Navigation.PushAsync(addRecipePage);
    }
}