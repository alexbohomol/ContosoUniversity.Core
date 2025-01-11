namespace Courses.Data.Reads;

using ContosoUniversity.Data;

using Core;

using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaReads(this IServiceCollection services)
    {
        services.AddDbContext<ReadOnlyContext>("Courses-RO");

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-courses-reads",
            tags: ["db", "sql", "courses", "reads"]);
    }
}
