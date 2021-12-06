namespace ContosoUniversity.Data.Courses.Writes;

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class SchemaMigrator
{
    /// <summary>
    ///     Ensure the database status for the host (created/migrated/initialized)
    ///     https://stackoverflow.com/a/55971168
    ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
    ///     https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#apply-migrations-at-runtime
    /// </summary>
    public static async Task EnsureCoursesSchema(this IWebHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ReadWriteContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
                await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            /*
             * TODO: DI resolution failing
             */
            var logger = services.GetRequiredService<ILogger>();
            logger.LogError(ex, "An error occurred while migrating the 'Courses' schema");
        }
    }
}