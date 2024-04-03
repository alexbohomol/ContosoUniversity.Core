namespace ContosoUniversity.Data.Courses.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaWrites(this IServiceCollection services, SqlConnectionStringBuilder builder)
    {
        services.AddDbContext<ReadWriteContext>(options =>
        {
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<ICoursesRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-courses-writes",
            tags: ["db", "sql", "courses", "writes"]);
    }
}
