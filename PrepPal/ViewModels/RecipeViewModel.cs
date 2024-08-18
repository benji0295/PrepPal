using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrepPal.Models;

namespace PrepPal.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        public RecipeViewModel()
        {
            LoadRecipes();
        }

        private void LoadRecipes()
        {
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
