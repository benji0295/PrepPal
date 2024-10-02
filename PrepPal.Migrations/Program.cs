using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrepPal.Data; 

namespace PrepPal.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build the host and services to enable DI for the DbContext
            var host = CreateHostBuilder(args).Build();

            // Execute the migrations
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Get the DbContext
                    var context = services.GetRequiredService<PrepPalDbContext>();

                    // Apply any pending migrations to the database
                    Console.WriteLine("Applying migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
                }
            }
        }

        // Set up the Host and DI container
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register the DbContext with the connection string
                    services.AddDbContext<PrepPalDbContext>(options =>
                        options.UseNpgsql("Host=localhost;Database=preppaldb;Username=bensmith;Password=bensmith",
                            b=> b.MigrationsAssembly("PrepPal.Migrations")));
                });
    }
}