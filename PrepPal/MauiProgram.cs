using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PrepPal.ViewModels;
using PrepPal.Views;
using PrepPal.Data;
using PrepPal.Data.CompiledModels;

namespace PrepPal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            var connectionString = "Host=localhost;Database=preppaldb;Username=bensmith;Password=bensmith";

            builder.Services.AddDbContext<PrepPalDbContext>(options =>
            {
                // Register the context with the PostgreSQL provider and the shared model
                options.UseNpgsql(connectionString)
                    .UseModel(PrepPalDbContextModel.Instance);
            });
            
            // Add other ViewModels and services
            builder.Services.AddTransient<RecipeViewModel>();
            builder.Services.AddSingleton<GroceryListViewModel>();
            builder.Services.AddTransient<FridgeListViewModel>();
            builder.Services.AddTransient<EditRecipeViewModel>();
            builder.Services.AddTransient<FavoriteRecipesViewModel>();

            // Register Pages for DI
            builder.Services.AddTransient<RecipePage>();
            builder.Services.AddTransient<RecipeDetailPage>();
            builder.Services.AddTransient<AddRecipePage>();
            builder.Services.AddTransient<FavoriteRecipePage>();
            builder.Services.AddTransient<GroceryListPage>();
            builder.Services.AddTransient<FridgeListPage>();

            // Register the AppShell
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
