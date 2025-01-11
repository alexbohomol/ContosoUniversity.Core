namespace ContosoUniversity.Data.Courses.Reads;

using Application.Contracts.Repositories.ReadOnly;

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
