namespace PrepPal.Views;

public partial class RecipeDetailPage : ContentPage
{
    private RecipeViewModel _recipeViewModel;
    private GroceryListViewModel _groceryListViewModel;
    public Recipe SelectedRecipe { get; set; }

    public RecipeDetailPage(Recipe selectedRecipe, RecipeViewModel recipeViewModel, GroceryListViewModel groceryListViewModel)
    {
        InitializeComponent();
        SelectedRecipe = selectedRecipe;
        _recipeViewModel = recipeViewModel;
        _groceryListViewModel = groceryListViewModel;
        AdjustImageWidth();

        BindingContext = SelectedRecipe;

        UpdateFavoriteIcon();
    }

    private void AdjustImageWidth()
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            RecipeImage.WidthRequest = Application.Current.MainPage.Width;
        }
    }

    private async void OnDeleteRecipeClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete Recipe",
            $"Are you sure you want to delete your {SelectedRecipe.Name} recipe?", "Yes", "No");
        
        if (confirm)
        {
            _recipeViewModel.Recipes.Remove(SelectedRecipe);
            await DisplayAlert("Deleted", $"{SelectedRecipe.Name} recipe has been deleted.", "OK");
            await Navigation.PopAsync();
        }
    }

    private async void OnAddIngredientsToGroceryListClicked(object sender, EventArgs e)
    {
        foreach (var ingredient in SelectedRecipe.RecipeIngredients)
        {
            var existingItem = _groceryListViewModel.GroceryItems
                .FirstOrDefault(item => item.Name.Equals(ingredient.IngredientName, StringComparison.OrdinalIgnoreCase));
            
            if (existingItem == null)
            {
                var newItem = new GroceryItem 
                { 
                    Name = $"{ingredient.Quantity} {ingredient.Unit} {ingredient.IngredientName}",
                    Aisle = ingredient.Aisle ?? "Other", 
                    StorageLocation = ingredient.StorageLocation ?? "Other", 
                    IsBought = false 
                };

                _groceryListViewModel.GroceryItems.Add(newItem);
            }
        }
        _groceryListViewModel.GroupItems();
        _groceryListViewModel.OnPropertyChanged(nameof(_groceryListViewModel.GroceryItems));
        
        await DisplayAlert("Success", "Ingredients added to your grocery list.", "OK");
    }

    private void OnFavoriteClicked(object sender, EventArgs e)
    {
        SelectedRecipe.IsFavorite = !SelectedRecipe.IsFavorite;

        if (SelectedRecipe.IsFavorite)
        {
            // Add to favorites
            if (!App.FavoriteRecipesViewModel.FavoriteRecipes.Contains(SelectedRecipe))
            {
                FavoriteToolbarItem.IconImageSource = "heart_icon_filled.png";
                App.FavoriteRecipesViewModel.FavoriteRecipes.Add(SelectedRecipe);
            }
        }
        else
        {
            // Remove from favorites
            if (App.FavoriteRecipesViewModel.FavoriteRecipes.Contains(SelectedRecipe))
            {
                FavoriteToolbarItem.IconImageSource = "heart_icon.png";
                App.FavoriteRecipesViewModel.FavoriteRecipes.Remove(SelectedRecipe);
            }
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFavoriteIcon();

        foreach (var ingredient in SelectedRecipe.RecipeIngredients)
        {
            ingredient.IsSelected = false;
        }

        foreach (var instruction in SelectedRecipe.Instructions)
        {
            instruction.IsCompleted = false;
        }
    }
    async void OnRecipeInfoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateRecipePage { BindingContext = BindingContext });
    }

    private void UpdateFavoriteIcon()
    {
        FavoriteToolbarItem.IconImageSource = SelectedRecipe.IsFavorite ? "heart_icon_filled.png" : "heart_icon.png";
    }
}
