using PrepPal.Models;

namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
	public RecipeDetailPage(Recipe selectedRecipe)
	{
		InitializeComponent();
		BindingContext = selectedRecipe;
	}
	private void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
	{
		var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;
		if (selectedRecipe == null)
		{
			Navigation.PushAsync(new RecipeDetailPage(selectedRecipe));
		}
	}
}