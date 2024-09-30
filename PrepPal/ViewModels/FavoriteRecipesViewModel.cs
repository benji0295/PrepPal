using System.Collections.ObjectModel;
using PrepPal.Models;
using System.ComponentModel;

namespace PrepPal.ViewModels;

public class FavoriteRecipesViewModel : INotifyPropertyChanged
{
    private static FavoriteRecipesViewModel _instance;

    public static FavoriteRecipesViewModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FavoriteRecipesViewModel();
            }

            return _instance;
        }
    }
    public ObservableCollection<Recipe> FavoriteRecipes { get; set; }

    public FavoriteRecipesViewModel()
    {
        FavoriteRecipes = new ObservableCollection<Recipe>();
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