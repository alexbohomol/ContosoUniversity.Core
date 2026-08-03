namespace ContosoUniversity.Data.Courses.Writes;

using Application.Contracts.Repositories.Writes;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddCoursesSchemaWrites(this IServiceCollection services)
    {
        services.AddDbContext<ReadWriteContext>((provider, options) =>
        {
            var config = provider.GetService<IOptionsMonitor<SqlConnectionStringBuilder>>();
            SqlConnectionStringBuilder builder = config.Get("Courses-RW");
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<ICoursesRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-courses-writes",
            tags: ["db", "sql", "courses", "writes"]);
    }
}
