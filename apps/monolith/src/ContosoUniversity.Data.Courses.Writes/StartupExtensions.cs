namespace ContosoUniversity.Data.Courses.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaWrites(this IServiceCollection services)
    {
        services.AddDbContext<ReadWriteContext>("Courses-RW");

        services.AddScoped<ICoursesRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-courses-writes",
            tags: ["db", "sql", "courses", "writes"]);
    }
}
