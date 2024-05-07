namespace ContosoUniversity.Data.Courses.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaReads(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var connectionResolver = provider.GetService<IConnectionResolver>();

            var builder = new DbContextOptionsBuilder<ReadOnlyContext>();
            var connectionString = connectionResolver.CreateFor("Courses-RO");

            builder.UseSqlServer(connectionString.ConnectionString);

            return new ReadOnlyContext(builder.Options);
        });

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-courses-reads",
            tags: ["db", "sql", "courses", "reads"]);
    }
}
