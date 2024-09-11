using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PrepPal.Models;
using PrepPal.Contexts;

namespace PrepPal.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe _selectedRecipe;
        private readonly PrepPalDbContext _context;

        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        public ObservableCollection<Recipe> FavoriteRecipes
        {
            get
            {
                return new ObservableCollection<Recipe>(Recipes.Where(r => r.IsFavorite));
            }
        }
        public ICommand IncreaseServingsCommand { get; set; }
        public ICommand DecreaseServingsCommand { get; set; }
        public ICommand ToggleFavoriteCommand { get; set; }

        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (_selectedRecipe != value)
                {
                    _selectedRecipe = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedRecipe));
                }
            }
        }
        public RecipeViewModel(PrepPalDbContext context)
        {
            _context = context;
            
            ToggleFavoriteCommand = new Command<Recipe>(ToggleFavorite);
            IncreaseServingsCommand = new Command(IncreaseServings);
            DecreaseServingsCommand = new Command(DecreaseServings);
            
            LoadRecipes();
        }

        private async void LoadRecipes()
        {
            var recipesFromDb = await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .ToListAsync();
            
            Recipes.Clear();
            foreach (var recipe in recipesFromDb)
            {
                Recipes.Add(recipe);
            }
            
            OnPropertyChanged(nameof(Recipes));
            OnPropertyChanged(nameof(FavoriteRecipes));
        }

        private async void ToggleFavorite(Recipe recipe)
        {
            if (recipe != null)
            {
                recipe.IsFavorite = !recipe.IsFavorite;

                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
                
                OnPropertyChanged(nameof(Recipes));
                OnPropertyChanged(nameof(FavoriteRecipes));
            }
        }

        private void IncreaseServings()
        {
            if (SelectedRecipe != null)
            {
                SelectedRecipe.Servings++;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }
        private void DecreaseServings()
        {
            if (SelectedRecipe != null && SelectedRecipe.Servings > 1)
            {
                SelectedRecipe.Servings--;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
