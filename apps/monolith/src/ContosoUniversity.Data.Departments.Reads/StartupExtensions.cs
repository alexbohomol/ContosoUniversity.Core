namespace ContosoUniversity.Data.Departments.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaReads(this IServiceCollection services)
    {
        services.AddDbContext<ReadOnlyContext>("Departments-RO");

        services.AddScoped<IDepartmentsRoRepository, DepartmentsReadOnlyRepository>();
        services.AddScoped<IInstructorsRoRepository, InstructorsReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-departments-reads",
            tags: ["db", "sql", "departments", "reads"]);
    }
}
