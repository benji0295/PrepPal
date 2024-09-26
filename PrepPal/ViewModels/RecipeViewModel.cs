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
        private readonly PrepPalDbContext _dbContext;
        private bool _isAllRecipesSelected = true;
        
        // Observable collections for the RecipeSelectionPage
        public ObservableCollection<Recipe> AllRecipes { get; set; }
        public ObservableCollection<Recipe> FilteredRecipes { get; set; }

        public bool IsFavoriteRecipesSelected => !IsAllRecipesSelected;
        public bool IsAllRecipesSelected
        {
            get => _isAllRecipesSelected;
            set
            {
                _isAllRecipesSelected = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
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
        public ICommand SwitchToAllRecipesCommand { get; }
        public ICommand SwitchToFavoriteRecipesCommand { get; }
        public ICommand NavigateBackCommand { get; }

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
        public RecipeViewModel(PrepPalDbContext dbContext)
        {
            _dbContext = dbContext;
            
            ToggleFavoriteCommand = new Command<Recipe>(ToggleFavorite);
            IncreaseServingsCommand = new Command(IncreaseServings);
            DecreaseServingsCommand = new Command(DecreaseServings);
            SwitchToAllRecipesCommand = new Command(SwitchToAllRecipes);
            SwitchToFavoriteRecipesCommand = new Command(SwitchToFavoriteRecipes);
            NavigateBackCommand = new Command(NavigateBack);

            Recipes = new ObservableCollection<Recipe>();
            AllRecipes = new ObservableCollection<Recipe>();
            FilteredRecipes = new ObservableCollection<Recipe>();
            
            LoadRecipes();
        }

        private async void LoadRecipes()
        {
            try
            {
                AllRecipes.Clear();
                
                var hardcodedRecipes = new ObservableCollection<Recipe>
                {
                    new Recipe
                    {
                        Name = "Chicken Alfredo",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "1 lb. fettuccine" },
                            new RecipeIngredient { IngredientName = "2 cups heavy cream" },
                            new RecipeIngredient { IngredientName = "1 cup grated Parmesan" },
                            new RecipeIngredient { IngredientName = "2 cloves garlic, minced" },
                            new RecipeIngredient { IngredientName = "1/2 cup butter" },
                            new RecipeIngredient { IngredientName = "1/2 tsp. salt" },
                            new RecipeIngredient { IngredientName = "1/4 tsp. pepper" },
                            new RecipeIngredient { IngredientName = "1/4 tsp. nutmeg" },
                            new RecipeIngredient { IngredientName = "1 lb. chicken breast, cooked and sliced" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction
                                { StepNumber = 1, Description = "Cook fettuccine according to package directions." },
                            new Instruction
                            {
                                StepNumber = 2,
                                Description =
                                    "In a saucepan, combine cream, Parmesan, garlic, butter, salt, pepper, and nutmeg."
                            },
                            new Instruction
                                { StepNumber = 3, Description = "Cook over medium heat until sauce thickens." },
                            new Instruction { StepNumber = 4, Description = "Add chicken and heat through." },
                            new Instruction { StepNumber = 5, Description = "Serve over fettuccine." }
                        },
                        Category = "Main Dish",
                        Servings = 4,
                        PrepTime = 10,
                        CookTime = 20,
                        TotalTime = 30,
                        Source = "Mom",
                        SourceURL = "example.com",
                        ImageURL = "chicken_alfredo.jpg"
                    },
                    new Recipe
                    {
                        Name = "Chocolate Chip Cookies",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "2 1/4 cups flour" },
                            new RecipeIngredient { IngredientName = "1 tsp. baking soda" },
                            new RecipeIngredient { IngredientName = "1 tsp. salt" },
                            new RecipeIngredient { IngredientName = "1 cup butter, softened" },
                            new RecipeIngredient { IngredientName = "3/4 cup sugar" },
                            new RecipeIngredient { IngredientName = "3/4 cup brown sugar" },
                            new RecipeIngredient { IngredientName = "1 tsp. vanilla" },
                            new RecipeIngredient { IngredientName = "2 eggs" },
                            new RecipeIngredient { IngredientName = "2 cups chocolate chips" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Preheat oven to 375°F." },
                            new Instruction
                            {
                                StepNumber = 2, Description = "In a small bowl, combine flour, baking soda, and salt."
                            },
                            new Instruction
                            {
                                StepNumber = 3,
                                Description = "In a large bowl, cream butter, sugar, brown sugar, and vanilla."
                            },
                            new Instruction
                            {
                                StepNumber = 4,
                                Description = "Add eggs one at a time, beating well after each addition."
                            },
                            new Instruction { StepNumber = 5, Description = "Gradually add flour mixture." },
                            new Instruction { StepNumber = 6, Description = "Stir in chocolate chips." },
                            new Instruction
                            {
                                StepNumber = 7,
                                Description = "Drop by rounded tablespoonfuls onto ungreased cookie sheet."
                            },
                            new Instruction
                                { StepNumber = 8, Description = "Bake for 9-11 minutes or until golden brown." }
                        },
                        Category = "Dessert",
                        Servings = 4,
                        PrepTime = 10,
                        CookTime = 10,
                        TotalTime = 20,
                        Source = "Grandma",
                        SourceURL = "example.com",
                        ImageURL = "cookies.jpg"
                    },
                };

                foreach (var recipe in hardcodedRecipes)
                {
                    Recipes.Add(recipe);
                    AllRecipes.Add(recipe);
                }

                var databaseRecipes = await _dbContext.Recipes
                    .Include(r => r.RecipeIngredients)
                    .Include(r => r.Instructions)
                    .ToListAsync();
                
                Console.WriteLine($"Database recipes count: {databaseRecipes.Count}");

                foreach (var recipe in databaseRecipes)
                {
                    Console.WriteLine($"Recipe: {recipe.Name}");
                    Recipes.Add(recipe);
                    AllRecipes.Add(recipe);
                }
                
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading recipes: {ex.Message}");
            }
        }
        
        private void ApplyFilter()
        {
            FilteredRecipes.Clear();

            if (IsAllRecipesSelected)
            {
                foreach (var recipe in AllRecipes)
                {
                    FilteredRecipes.Add(recipe);
                }
            }
            else
            {
                foreach (var recipe in AllRecipes.Where(r => r.IsFavorite))
                {
                    FilteredRecipes.Add(recipe);
                }
            }
        }

        private void ToggleFavorite(Recipe recipe)
        {
            if (recipe != null)
            {
                recipe.IsFavorite = !recipe.IsFavorite;
                OnPropertyChanged(nameof(Recipes));
            }
        }
        private void SwitchToAllRecipes()
        {
            IsAllRecipesSelected = true;
        }

        private void SwitchToFavoriteRecipes()
        {
            IsAllRecipesSelected = false;
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

        private async void NavigateBack()
        {
            await Shell.Current.GoToAsync("//MealPlanPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
