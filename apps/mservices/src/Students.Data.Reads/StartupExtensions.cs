namespace Students.Data.Reads;

using ContosoUniversity.Data;

using Core;

using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaReads(this IServiceCollection services)
    {
        services.AddDbContext<ReadOnlyContext>("Students-RO");

        services.AddScoped<IStudentsRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-students-reads",
            tags: ["db", "sql", "students", "reads"]);
    }
}
