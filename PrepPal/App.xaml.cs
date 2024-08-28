using PrepPal.ViewModels;

namespace PrepPal
{
    public partial class App : Application
    {
        public static GroceryListViewModel GroceryListViewModel { get; private set; }
        public static FavoriteRecipesViewModel FavoriteRecipesViewModel { get; private set; }
        public App()
        {
            InitializeComponent();
            GroceryListViewModel = new GroceryListViewModel();
            FavoriteRecipesViewModel = new FavoriteRecipesViewModel();
            MainPage = new AppShell();
        }
    }
}
