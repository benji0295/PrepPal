using PrepPal.Repositories;

namespace PrepPal.Views;

[QueryProperty(nameof(Day), "day")]
public partial class RecipeSelectionPage : ContentPage
{
    public string Day { get; set; }
    private RecipeRepository _recipeRepository;

    public RecipeSelectionPage()
    {
        InitializeComponent();
        
        BindingContext = new RecipeViewModel(_recipeRepository);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        var viewModel = BindingContext as RecipeViewModel;
        if (viewModel != null)
        {
            viewModel.IsAllRecipesSelected = true;
            viewModel.UpdatedFilteredRecipes();
        }

        recipeCollectionView.SelectedItem = null;
    }
    
    public RecipeSelectionPage(RecipeRepository recipeRepository)
    {
        InitializeComponent();
        _recipeRepository = recipeRepository;
        BindingContext = new RecipeViewModel(recipeRepository);
    }

    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedRecipe = e.CurrentSelection.FirstOrDefault() as Recipe;

        if (selectedRecipe != null)
        {
            (sender as CollectionView).SelectedItem = null;
            
            await Shell.Current.GoToAsync($"//MealPlanPage?recipe={selectedRecipe.Name}&day={Day}&image={selectedRecipe.ImageURL}");
        }
    }
}