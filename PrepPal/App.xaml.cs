namespace PrepPal
{
    public partial class App : Application
    {
        public static GroceryListViewModel GroceryListViewModel { get; private set; }
        public static FridgeListViewModel FridgeListViewModel { get; private set; }
        public static FavoriteRecipesViewModel FavoriteRecipesViewModel { get; private set; }
        private static SharedService _sharedService;
        
        private static NpgsqlConnection _connection;
        public App()
        {
            InitializeComponent();

            try
            {
                InitializeDatabaseConnection();
            }
            catch (Exception ex)
            {
                // Display the error or log it
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
            
            _sharedService = new SharedService();
            
            FridgeListViewModel = new FridgeListViewModel(_sharedService);
            GroceryListViewModel = new GroceryListViewModel(_sharedService);
            FavoriteRecipesViewModel = new FavoriteRecipesViewModel();
            
            MainPage = new AppShell();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Host=localhost;Database=preppaldb;Username=bensmith;Password=bensmith";
            _connection = new NpgsqlConnection(connectionString);
            try
            {
                _connection.Open();
                Console.WriteLine("Database connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }
        }

        public static NpgsqlConnection GetDatabaseConnection()
        {
            return _connection;
        }
    }
}
