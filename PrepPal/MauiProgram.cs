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
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var connectionString = config.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<PrepPalDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseModel(PrepPalDbContextModel.Instance)
                    .LogTo(Console.WriteLine, LogLevel.Information));
            
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
            builder.Services.AddSingleton<SharedService>();
            
            builder.Services.AddScoped<RecipeRepository>();

            // Register the AppShell
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
