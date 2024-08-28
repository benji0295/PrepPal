using System.Collections.ObjectModel;
using PrepPal.Models;
using System.ComponentModel;

namespace PrepPal.ViewModels;

public class FavoriteRecipesViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Recipe> FavoriteRecipes { get; set; }

    public FavoriteRecipesViewModel()
    {
        FavoriteRecipes = new ObservableCollection<Recipe>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}