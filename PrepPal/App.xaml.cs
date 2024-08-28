using PrepPal.ViewModels;

namespace PrepPal
{
    public partial class App : Application
    {
        public static GroceryListViewModel GroceryListViewModel { get; private set; }
        public App()
        {
            InitializeComponent();
            GroceryListViewModel = new GroceryListViewModel();
            MainPage = new AppShell();
        }
    }
}
