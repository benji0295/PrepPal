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
using PrepPal.Views;

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
                            new RecipeIngredient { IngredientName = "Fettuccine", Quantity = 1, Unit = "lb"  },
                            new RecipeIngredient { IngredientName = "Heavy cream", Quantity = 2, Unit = "cups" },
                            new RecipeIngredient { IngredientName = "Grated Parmesan", Quantity = 1, Unit = "cup" },
                            new RecipeIngredient { IngredientName = "Garlic, minced", Quantity = 2, Unit = "cloves" },
                            new RecipeIngredient { IngredientName = "Butter", Quantity = 0.5m, Unit = "cup" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0.5m, Unit = "tsp" },
                            new RecipeIngredient { IngredientName = "Pepper", Quantity = 0.25m, Unit = "tsp" },
                            new RecipeIngredient { IngredientName = "Nutmeg", Quantity = 0.25m, Unit = "tsp" },
                            new RecipeIngredient { IngredientName = "Chicken breast, cooked and sliced", Quantity = 1, Unit = "lb" }
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
                            new RecipeIngredient { IngredientName = "Flour", Quantity = 2.25m, Unit = "cups" },
                            new RecipeIngredient { IngredientName = "Baking soda", Quantity = 1, Unit = "tsp" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 1, Unit = "tsp" },
                            new RecipeIngredient { IngredientName = "Butter, softened", Quantity = 1, Unit = "cup" },
                            new RecipeIngredient { IngredientName = "Sugar", Quantity = 0.75m, Unit = "cup" },
                            new RecipeIngredient { IngredientName = "Brown sugar", Quantity = 0.75m, Unit = "cup" },
                            new RecipeIngredient { IngredientName = "Vanilla", Quantity = 1, Unit = "tsp" },
                            new RecipeIngredient { IngredientName = "Eggs", Quantity = 2 },
                            new RecipeIngredient { IngredientName = "Chocolate chips", Quantity = 2, Unit = "cups" }
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
            OnPropertyChanged(nameof(FilteredRecipes));
        }

        public void UpdatedFilteredRecipes()
        {
            ApplyFilter();
        }

        private void ToggleFavorite(Recipe recipe)
        {
            if (recipe != null)
            {
                recipe.IsFavorite = !recipe.IsFavorite;
                OnPropertyChanged(nameof(Recipes));
                ApplyFilter();
            }
        }
        private void SwitchToAllRecipes()
        {
            IsAllRecipesSelected = true;
            ApplyFilter();
        }

        private void SwitchToFavoriteRecipes()
        {
            IsAllRecipesSelected = false;
            ApplyFilter();
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
            try
            {
                // Check the current page route
                var currentRoute = Shell.Current.CurrentState.Location.ToString();
        
                // If the current route is RecipeSelectionPage, go back to MealPlanPage
                if (currentRoute.Contains("RecipeSelectionPage"))
                {
                    await Shell.Current.GoToAsync("//MealPlanPage");
                }
                // If the current route is RecipeDetailPage, go back to RecipePage
                else if (currentRoute.Contains("RecipeDetailPage"))
                {
                    await Shell.Current.GoToAsync("//RecipePage");
                }
                // If the current route is AddRecipePage, go back to RecipePage
                else if (currentRoute.Contains("AddRecipePage"))
                {
                    await Shell.Current.GoToAsync("//RecipePage");
                }
                // Fallback to default (RecipePage) if no conditions are met
                else
                {
                    await Shell.Current.GoToAsync("//RecipePage");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"NavigateBack exception: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
