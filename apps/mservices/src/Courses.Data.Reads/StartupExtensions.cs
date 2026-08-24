namespace Courses.Data.Reads;

using Core;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddCoursesSchemaReads(this IServiceCollection services)
    {
        services.AddDbContext<ReadOnlyContext>((provider, options) =>
        {
            var config = provider.GetService<IOptionsMonitor<SqlConnectionStringBuilder>>();
            SqlConnectionStringBuilder builder = config.Get("Courses-RO");
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-courses-reads",
            tags: ["db", "sql", "courses", "reads"]);
    }
}
