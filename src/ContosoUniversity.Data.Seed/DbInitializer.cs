using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data.Courses;
using ContosoUniversity.Data.Departments;
using ContosoUniversity.Data.Students;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity.Data.Seed;

public static class DbInitializer
{
    /// <summary>
    ///     Ensure the database status for the host (created/migrated/initialized)
    ///     https://stackoverflow.com/a/55971168
    ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
    ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#apply-migrations-at-runtime
    /// </summary>
    public static async Task EnsureDataLayer(this IWebHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var coursesContext = services.GetRequiredService<CoursesContext>();
            if ((await coursesContext.Database.GetPendingMigrationsAsync()).Any())
                await coursesContext.Database.MigrateAsync();

            var studentsContext = services.GetRequiredService<StudentsContext>();
            if ((await studentsContext.Database.GetPendingMigrationsAsync()).Any())
                await studentsContext.Database.MigrateAsync();

            var departmentsContext = services.GetRequiredService<DepartmentsContext>();
            if ((await departmentsContext.Database.GetPendingMigrationsAsync()).Any())
                await departmentsContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            /*
             * TODO: DI resolution failing
             */
            var logger = services.GetRequiredService<ILogger>();
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }
}