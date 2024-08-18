using PrepPal.ViewModels;

namespace PrepPal;

public partial class RecipePage : ContentPage
{
	public RecipePage()
	{
		InitializeComponent();
		BindingContext = new RecipeViewModel();
	}
}