using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PrepPal.Contexts;

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
            
            // Connection string for PostgreSQL
            var connectionString = "Host=localhost;Database=preppaldb;Username=bensmith;Password=bensmith";

            // Register the DbContext with PostgreSQL
            builder.Services.AddDbContext<PrepPalDbContext>(options =>
                options.UseNpgsql(connectionString));

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
