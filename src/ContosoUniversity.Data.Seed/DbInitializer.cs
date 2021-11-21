namespace ContosoUniversity.Data.Seed;

using System;
using System.Linq;

using Courses;

using Departments;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Students;

public static class DbInitializer
{
    /// <summary>
    ///     Ensure the database status for the host (created/migrated/initialized)
    ///     https://stackoverflow.com/a/55971168
    ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
    ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#apply-migrations-at-runtime
    /// </summary>
    public static void EnsureDataLayer(this IWebHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            var coursesContext = services.GetRequiredService<CoursesContext>();
            if (coursesContext.Database.GetPendingMigrations().Any()) coursesContext.Database.Migrate();

            var studentsContext = services.GetRequiredService<StudentsContext>();
            if (studentsContext.Database.GetPendingMigrations().Any()) studentsContext.Database.Migrate();

            var departmentsContext = services.GetRequiredService<DepartmentsContext>();
            if (departmentsContext.Database.GetPendingMigrations().Any()) departmentsContext.Database.Migrate();

            DbSeeder.EnsureInitialized(departmentsContext, coursesContext, studentsContext);
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