using PrepPal.ViewModels;

namespace PrepPal
{
    public partial class App : Application
    {
        public static GroceryListViewModel GroceryListViewModel { get; private set; }
        public static FridgeListViewModel FridgeListViewModel { get; private set; }
        public static FavoriteRecipesViewModel FavoriteRecipesViewModel { get; private set; }
        public App()
        {
            InitializeComponent();
            
            FridgeListViewModel = new FridgeListViewModel();
            GroceryListViewModel = new GroceryListViewModel(FridgeListViewModel);
            FavoriteRecipesViewModel = new FavoriteRecipesViewModel();
            
            MainPage = new AppShell();
        }
    }
}
