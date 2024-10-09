namespace PrepPal
{
    public partial class App : Application
    {
        public static GroceryListViewModel GroceryListViewModel { get; private set; }
        public static FridgeListViewModel FridgeListViewModel { get; private set; }
        public static FavoriteRecipesViewModel FavoriteRecipesViewModel { get; private set; }
        public static IServiceProvider Services { get; private set; }
        
        public App()
        {
            InitializeComponent();

            try
            {
                Services = MauiProgram.CreateMauiApp().Services;
             
            }
            catch (Exception ex)
            {
                // Display the error or log it
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
            
            var recipePage = App.Services.GetRequiredService<RecipePage>();
            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
            
            var groceryListViewModel = Services.GetService<GroceryListViewModel>();
            var fridgeListViewModel = Services.GetService<FridgeListViewModel>();
            var favoriteRecipesViewModel = Services.GetService<FavoriteRecipesViewModel>();

            
            MainPage = Services.GetService<AppShell>();
        }
        public static T GetViewModel<T>() where T : class
        {
            return Services.GetService<T>();
        }
    }
}
