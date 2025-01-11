namespace ContosoUniversity.Data.Students.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaWrites(this IServiceCollection services)
    {
        services.AddDbContext<ReadWriteContext>("Students-RW");

        services.AddScoped<IStudentsRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-students-writes",
            tags: ["db", "sql", "students", "writes"]);
    }
}
