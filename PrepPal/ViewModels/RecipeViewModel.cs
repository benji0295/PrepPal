using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe _selectedRecipe;

        public ObservableCollection<Recipe> Recipes { get; set; }

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
        public RecipeViewModel()
        {
            ToggleFavoriteCommand = new Command<Recipe>(ToggleFavorite);
            IncreaseServingsCommand = new Command(IncreaseServings);
            DecreaseServingsCommand = new Command(DecreaseServings);
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            Recipes = new ObservableCollection<Recipe>
            {
                new Recipe
                {
                    Name = "Chicken Alfredo",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "1 lb. fettuccine" },
                        new Ingredient { Name = "2 cups heavy cream" },
                        new Ingredient { Name = "1 cup grated Parmesan" },
                        new Ingredient { Name = "2 cloves garlic, minced" },
                        new Ingredient { Name = "1/2 cup butter" },
                        new Ingredient { Name = "1/2 tsp. salt" },
                        new Ingredient { Name = "1/4 tsp. pepper" },
                        new Ingredient { Name = "1/4 tsp. nutmeg" },
                        new Ingredient { Name = "1 lb. chicken breast, cooked and sliced" }
                    },
                    Instructions = new List<Instruction>
                    {
                        new Instruction { Step = "Cook fettuccine according to package directions." },
                        new Instruction
                        {
                            Step = "In a saucepan, combine cream, Parmesan, garlic, butter, salt, pepper, and nutmeg."
                        },
                        new Instruction { Step = "Cook over medium heat until sauce thickens." },
                        new Instruction { Step = "Add chicken and heat through." },
                        new Instruction { Step = "Serve over fettuccine." }
                    },
                    Category = "Main Dish",
                    Servings = 4,
                    PrepTime = 10,
                    CookTime = 20,
                    TotalTime = 30,
                    Source = "Mom",
                    SourceURL = "example.com"
                },
                new Recipe
                {
                    Name = "Chocolate Chip Cookies",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "2 1/4 cups flour" },
                        new Ingredient { Name = "1 tsp. baking soda" },
                        new Ingredient { Name = "1 tsp. salt" },
                        new Ingredient { Name = "1 cup butter, softened" },
                        new Ingredient { Name = "3/4 cup sugar" },
                        new Ingredient { Name = "3/4 cup brown sugar" },
                        new Ingredient { Name = "1 tsp. vanilla" },
                        new Ingredient { Name = "2 eggs" },
                        new Ingredient { Name = "2 cups chocolate chips" }
                    },
                    Instructions = new List<Instruction>
                    {
                        new Instruction { Step = "Preheat oven to 375°F." },
                        new Instruction { Step = "In a small bowl, combine flour, baking soda, and salt." },
                        new Instruction { Step = "In a large bowl, cream butter, sugar, brown sugar, and vanilla." },
                        new Instruction { Step = "Add eggs one at a time, beating well after each addition." },
                        new Instruction { Step = "Gradually add flour mixture." },
                        new Instruction { Step = "Stir in chocolate chips." },
                        new Instruction { Step = "Drop by rounded tablespoonfuls onto ungreased cookie sheet." },
                        new Instruction { Step = "Bake for 9-11 minutes or until golden brown." }
                    },
                    Category = "Dessert",
                    Servings = 4,
                    PrepTime = 10,
                    CookTime = 10,
                    TotalTime = 20,
                    Source = "Grandma",
                    SourceURL = "example.com"
                },
            };
        }

        private void ToggleFavorite(Recipe recipe)
        {
            if (recipe != null)
            {
                recipe.IsFavorite = !recipe.IsFavorite;
                OnPropertyChanged(nameof(Recipes));
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
