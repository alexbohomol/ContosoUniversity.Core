namespace ContosoUniversity
{
    using System;
    using System.Linq;

    using Data;
    using Data.Contexts;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            EnsureDatabaseFor(host);

            host.Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }

        /// <summary>
        ///     Ensure the database status for the host (created/migrated/initialized)
        ///     https://stackoverflow.com/a/55971168
        ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
        ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#apply-migrations-at-runtime
        /// </summary>
        private static void EnsureDatabaseFor(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var coursesContext = services.GetRequiredService<CoursesContext>();
                    if (coursesContext.Database.GetPendingMigrations().Any())
                    {
                        coursesContext.Database.Migrate();
                    }

                    var schoolContext = services.GetRequiredService<SchoolContext>();
                    if (schoolContext.Database.GetPendingMigrations().Any())
                    {
                        schoolContext.Database.Migrate();
                    }
                    
                    DbInitializer.EnsureInitialized(schoolContext, coursesContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}