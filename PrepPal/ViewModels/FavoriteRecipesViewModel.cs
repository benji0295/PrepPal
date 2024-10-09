using System.Collections.ObjectModel;
using PrepPal.Models;
using System.ComponentModel;

namespace PrepPal.ViewModels;

public class FavoriteRecipesViewModel : INotifyPropertyChanged
{
    private readonly RecipeRepository _recipeRepository;
    
    public ObservableCollection<Recipe> FavoriteRecipes { get; set; }

    public FavoriteRecipesViewModel(RecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
        FavoriteRecipes = new ObservableCollection<Recipe>();
        LoadFavoriteRecipesAsync();
    }
    private async void LoadFavoriteRecipesAsync()
    {
        var recipes = await _recipeRepository.GetFavoriteRecipesAsync();
        foreach (var recipe in recipes)
        {
            FavoriteRecipes.Add(recipe);
        }
        OnPropertyChanged(nameof(FavoriteRecipes));
    }
    public void AddToFavorites(Recipe recipe)
    {
        if (recipe != null && !FavoriteRecipes.Any(r => r.RecipeId == recipe.RecipeId))
        {
            FavoriteRecipes.Add(recipe);
            OnPropertyChanged(nameof(FavoriteRecipes));
        }
    }
    public void RemoveFromFavorites(Recipe recipe)
    {
        if (recipe != null && FavoriteRecipes.Contains(recipe))
        {
            FavoriteRecipes.Remove(recipe);
            OnPropertyChanged(nameof(FavoriteRecipes));
        }
    }
    public bool IsFavorite(Recipe recipe)
    {
        return FavoriteRecipes.Any(r => r.RecipeId == recipe.RecipeId);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}