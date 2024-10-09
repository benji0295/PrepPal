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
        public ICommand OpenSourceUrlCommand { get; }

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
            OpenSourceUrlCommand = new Command(OpenSourceUrl);

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
                            new RecipeIngredient { IngredientName = "Fettuccine", Quantity = 1, Unit = "lb", Aisle = "Pasta", StorageLocation  = "Pantry"},
                            new RecipeIngredient { IngredientName = "Heavy cream", Quantity = 2, Unit = "cups", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Grated Parmesan", Quantity = 1, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Garlic, minced", Quantity = 2, Unit = "cloves", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Butter", Quantity = 0.5m, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0.5m, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Pepper", Quantity = 0.25m, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Nutmeg", Quantity = 0.25m, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Chicken breast, cooked and sliced", Quantity = 1, Unit = "lb", Aisle = "Meat", StorageLocation  = "Fridge" }
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
                            new RecipeIngredient { IngredientName = "Flour", Quantity = 2.25m, Unit = "cups", Aisle = "Baking", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Baking soda", Quantity = 1, Unit = "tsp", Aisle = "Baking", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 1, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Butter, softened", Quantity = 1, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Sugar", Quantity = 0.75m, Unit = "cup", Aisle = "Baking", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Brown sugar", Quantity = 0.75m, Unit = "cup", Aisle = "Baking", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Vanilla", Quantity = 1, Unit = "tsp", Aisle = "Baking", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Eggs", Quantity = 2, Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Chocolate chips", Quantity = 2, Unit = "cups", Aisle = "Baking", StorageLocation  = "Pantry" }
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
                    new Recipe
                    {
                        Name = "Spicy Vodka Pasta",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Butter (vegan or regular)", Quantity = 3, Unit = "tbsp", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Shallots, chopped", Quantity = 2, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Garlic, minced", Quantity = 2, Unit = "cloves", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Tomato paste", Quantity = 0.25m, Unit = "cup", Aisle = "Canned Goods", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Vodka", Quantity = 1, Unit = "tbsp", Aisle = "Liquor", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Heavy cream", Quantity = 0.5m, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Chili flakes", Quantity = 1, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Salt and pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Rigatoni (dry)", Quantity = 225, Unit = "g", Aisle = "Pasta", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Parmesan cheese", Quantity = 0.25m, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Fresh basil", Quantity = 0, Unit = "for garnish", Aisle = "Spices", StorageLocation  = "Pantry" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Bring a large pot of water to a boil and cook pasta according to package directions. Reserve 1 cup pasta water before draining. While pasta is cooking, make the vodka sauce." },
                            new Instruction { StepNumber = 2, Description = "In a large saucepan, heat the butter, garlic and shallot over medium heat. Cook until softened, 3-5 minutes." },
                            new Instruction { StepNumber = 3, Description = "Add tomato paste, cook until the sauce is darker and a bit caramelized, 2-3 minutes." },
                            new Instruction { StepNumber = 4, Description = "Add in vodka and cook it until evaporated." },
                            new Instruction { StepNumber = 5, Description = "Then add heavy cream and 1 tsp chili flakes. Keep stirring until combined and season to taste with salt and pepper." },
                            new Instruction { StepNumber = 6, Description = "If pasta is not done cooking yet, remove sauce from the heat." },
                            new Instruction { StepNumber = 7, Description = "When pasta is done cooking, add 1/2 cup reserved pasta water to the vodka sauce and 1 tbsp butter. Stir over medium heat until butter is melted and sauce is smooth and creamy – add another ¼ cup pasta water if needed." },
                            new Instruction { StepNumber = 8, Description = "Add strained pasta and ¼ cup Parmesan cheese and stir. Remove from heat and adjust seasoning with more salt and pepper if needed." },
                            new Instruction { StepNumber = 9, Description = "Garnish with fresh basil and more Parmesan cheese if desired." }
                        },
                        Category = "Main Dish",
                        Servings = 2,
                        PrepTime = 10,
                        CookTime = 20,
                        TotalTime = 30,
                        Source = "Food By Maria",
                        SourceURL = "https://www.foodbymaria.com/spicy-vodka-pasta/",
                        ImageURL = "vodka_pasta.jpg"
                    },
                    new Recipe
                    {
                        Name = "Chicken Enchiladas",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Tomatillos, husked and rinsed", Quantity = 9, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "White onion, medium", Quantity = 0.5m, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Serrano chile", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Yellow chile (guero)", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Garlic cloves", Quantity = 2, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Fresh cilantro leaves, loosely packed", Quantity = 0.5m, Unit = "cup", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Freshly ground black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Vegetable oil", Quantity = 0.25m, Unit = "cup", Aisle = "Oils", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Corn tortillas (6-inch)", Quantity = 6, Unit = "pcs", Aisle = "Bread", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Rotisserie chicken breasts, skinned and shredded", Quantity = 1.5m, Unit = "cups", Aisle = "Meat", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Mexican crema or sour cream", Quantity = 0.5m, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Shredded Monterey Jack cheese", Quantity = 1, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Preheat the oven to 350°F." },
                            new Instruction { StepNumber = 2, Description = "Put the tomatillos, onion, serrano, yellow chile, and 3/4 cup water in a medium saucepan. Bring to a boil over medium-high heat." },
                            new Instruction { StepNumber = 3, Description = "Cover and boil until the tomatillos turn olive-green, about 10 minutes." },
                            new Instruction { StepNumber = 4, Description = "Transfer the tomatillos, onion, and chiles to a blender. Add the garlic and cilantro and blend until smooth. Season with salt and pepper." },
                            new Instruction { StepNumber = 5, Description = "Heat the oil in a small skillet over medium-high heat. Fry the tortillas until golden but still pliable, about 10 seconds per side. Transfer to paper towels to drain." },
                            new Instruction { StepNumber = 6, Description = "Put the tortillas on a work surface. Divide the shredded chicken evenly among the tortillas and roll up each like a cigar." },
                            new Instruction { StepNumber = 7, Description = "Spread 1/3 cup sauce in a 9x13-inch glass baking dish. Arrange the enchiladas, seam-side down, snugly inside the dish." },
                            new Instruction { StepNumber = 8, Description = "Pour the remaining sauce over the enchiladas. Drizzle with Mexican crema and sprinkle cheese all over." },
                            new Instruction { StepNumber = 9, Description = "Bake until the cheese melts and starts to brown in spots, about 30 minutes. Serve immediately." }
                        },
                        Category = "Main Dish",
                        Servings = 4,
                        PrepTime = 25,
                        CookTime = 40,
                        TotalTime = 65,
                        Source = "Food Network",
                        SourceURL = "https://www.foodnetwork.com/recipes/marcela-valladolid/chicken-enchiladas-recipe-1924424",
                        ImageURL = "chicken_enchiladas.png"
                    },
                    new Recipe
                    {
                        Name = "Slow Cooker Cheesy Breakfast Potatoes",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Russet potatoes, peeled and diced", Quantity = 3, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Red bell pepper, diced", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Green bell pepper, diced", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Onion, diced", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Smoked andouille chicken sausage, thinly sliced", Quantity = 12.8m, Unit = "oz", Aisle = "Meat", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Shredded cheddar cheese", Quantity = 1.5m, Unit = "cups", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Sour cream", Quantity = 0.5m, Unit = "cup", Aisle = "Dairy", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Dried oregano", Quantity = 0.25m, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Dried basil", Quantity = 0.25m, Unit = "tsp", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Condensed cream of chicken soup", Quantity = 10.75m, Unit = "oz", Aisle = "Canned Goods", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Kosher salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Freshly ground black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Fresh parsley leaves, chopped", Quantity = 2, Unit = "tbsp", Aisle = "Produce", StorageLocation  = "Fridge" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Place potatoes, bell peppers, onion, chicken sausage, cheese, sour cream, oregano and basil into a 6-qt slow cooker." },
                            new Instruction { StepNumber = 2, Description = "Stir in chicken soup; season with salt and pepper, to taste." },
                            new Instruction { StepNumber = 3, Description = "Cover and cook on low heat for 4-5 hours or high heat for 2-3 hours." },
                            new Instruction { StepNumber = 4, Description = "Serve immediately, garnished with parsley, if desired." }
                        },
                        Category = "Breakfast",
                        Servings = 8,
                        PrepTime = 15,
                        CookTime = 240,
                        TotalTime = 255,
                        Source = "Damn Delicious",
                        SourceURL = "https://damndelicious.net/2015/12/04/slow-cooker-cheesy-breakfast-potatoes/",
                        ImageURL = "breakfast_potatoes.jpg"
                    },
                    new Recipe
                    {
                        Name = "Chicken Stir-Fry",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Reduced-sodium soy sauce", Quantity = 0.5m, Unit = "cup", Aisle = "Condiments", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Honey", Quantity = 2, Unit = "tbsp", Aisle = "Condiments", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Toasted sesame oil", Quantity = 2, Unit = "tsp", Aisle = "Oils", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Canola oil", Quantity = 1, Unit = "tbsp", Aisle = "Oils", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Broccoli, cut into small florets", Quantity = 1, Unit = "head", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Bell pepper, seeds and ribs removed, chopped", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Garlic cloves, finely chopped", Quantity = 2, Unit = "pcs", Aisle = "Produce", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Boneless, skinless chicken breast, cut into 1\" pieces", Quantity = 1, Unit = "lb", Aisle = "Meat", StorageLocation  = "Fridge" },
                            new RecipeIngredient { IngredientName = "Cashews", Quantity = 0.33m, Unit = "cup", Aisle = "Snacks", StorageLocation  = "Pantry" },
                            new RecipeIngredient { IngredientName = "Freshly ground black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation  = "Pantry" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "In a small bowl, whisk soy sauce, honey, and sesame oil." },
                            new Instruction { StepNumber = 2, Description = "In a large skillet over high heat, heat canola oil. Cook broccoli, bell pepper, and garlic, stirring frequently, until softened, about 5 minutes." },
                            new Instruction { StepNumber = 3, Description = "Add chicken and cook, tossing occasionally, until golden brown and cooked through, about 8 minutes." },
                            new Instruction { StepNumber = 4, Description = "Stir in cashews; season with pepper." },
                            new Instruction { StepNumber = 5, Description = "Pour sauce into skillet and bring to a simmer. Cook, stirring occasionally, until thickened, about 5 minutes." }
                        },
                        Category = "Main Dish",
                        Servings = 4,
                        PrepTime = 10,
                        CookTime = 20, 
                        TotalTime = 30,
                        Source = "Delish",
                        SourceURL = "https://www.delish.com/cooking/recipe-ideas/a45362568/best-chicken-stir-fry-recipe/",
                        ImageURL = "stir_fry.png"
                    }
                };

                foreach (var recipe in hardcodedRecipes)
                {
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
                foreach (var recipe in Recipes.Where(r => r.IsFavorite))
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
                
                ApplyFilter();
                
                OnPropertyChanged(nameof(Recipes));
                OnPropertyChanged(nameof(FavoriteRecipes));
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
        private void OpenSourceUrl()
        {
            if (!string.IsNullOrEmpty(SelectedRecipe?.SourceURL))
            {
                try
                {
                    Uri uri = new Uri(SelectedRecipe.SourceURL);
                    Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening URL: {ex.Message}");
                }
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
