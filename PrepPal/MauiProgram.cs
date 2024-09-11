using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            
            builder.Services.AddDbContext<PrepPalDbContext>(options => 
                options.UseNpgsql("Host=localhost;Database=preppaldb")
            );

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
