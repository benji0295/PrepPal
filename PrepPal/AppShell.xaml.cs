
using PrepPal.Views;

namespace PrepPal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
            Routing.RegisterRoute(nameof(GroceryListPage), typeof(GroceryListPage));
        }
    }
}
