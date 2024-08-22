using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe _selectedRecipe;

        public ObservableCollection<Recipe> Recipes { get; set; }

        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (_selectedRecipe != value)
                {
                    _selectedRecipe = value;
                    OnPropertyChanged();
                }
            }
        }

        public RecipeViewModel()
        {
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            Recipes = new ObservableCollection<Recipe>();
            Recipes.Add(new Recipe
            {
                Name = "Chicken Alfredo",
                Ingredients = new List<string>
            {
                "1 lb. fettuccine",
                "2 cups heavy cream",
                "1 cup grated Parmesan",
                "2 cloves garlic, minced",
                "1/2 cup butter",
                "1/2 tsp. salt",
                "1/4 tsp. pepper",
                "1/4 tsp. nutmeg",
                "1 lb. chicken breast, cooked and sliced"
            },
                Instructions = new List<string>
            {
                "Cook fettuccine according to package directions.",
                "In a saucepan, combine cream, Parmesan, garlic, butter, salt, pepper, and nutmeg.",
                "Cook over medium heat until sauce thickens.",
                "Add chicken and heat through.",
                "Serve over fettuccine."
            },
                Category = "Main Dish",
                Servings = 4,
                PrepTime = 10,
                CookTime = 20,
                TotalTime = 30,
                Source = "Mom",
                SourceURL = "example.com"
            });
            Recipes.Add(new Recipe
            {
                Name = "Chocolate Chip Cookies",
                Ingredients = new List<string>
                {
                    "2 1/4 cups flour",
                    "1 tsp. baking soda",
                    "1 tsp. salt",
                    "1 cup butter, softened",
                    "3/4 cup sugar",
                    "3/4 cup brown sugar",
                    "1 tsp. vanilla",
                    "2 eggs",
                    "2 cups chocolate chips"
                },
                Instructions = new List<string> 
                {
                    "Preheat oven to 375°F.",
                    "In a small bowl, combine flour, baking soda, and salt.",
                    "In a large bowl, cream butter, sugar, brown sugar, and vanilla.",
                    "Add eggs one at a time, beating well after each addition.",
                    "Gradually add flour mixture.",
                    "Stir in chocolate chips.",
                    "Drop by rounded tablespoonfuls onto ungreased cookie sheet.",
                    "Bake for 9-11 minutes or until golden brown."
                },
                Category = "Dessert",
                Servings = 4,
                PrepTime = 10,
                CookTime = 10,
                TotalTime = 20,
                Source = "Grandma",
                SourceURL = "example.com"
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
