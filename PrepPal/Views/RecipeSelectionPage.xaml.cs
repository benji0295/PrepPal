namespace PrepPal.Views;

[QueryProperty(nameof(Day), "day")]
public partial class RecipeSelectionPage : ContentPage
{
    public string Day { get; set; }
    private readonly PrepPalDbContext _dbContext;

    public RecipeSelectionPage()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<PrepPalDbContext>()
            .UseNpgsql("DataSource=app.db")
            .Options;

        _dbContext = new PrepPalDbContext(options);
        
        BindingContext = new RecipeViewModel(_dbContext);
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
    
    public RecipeSelectionPage(PrepPalDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        BindingContext = new RecipeViewModel(_dbContext);
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