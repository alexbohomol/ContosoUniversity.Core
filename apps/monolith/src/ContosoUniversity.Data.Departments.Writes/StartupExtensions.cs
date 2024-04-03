namespace ContosoUniversity.Data.Departments.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaWrites(this IServiceCollection services, SqlConnectionStringBuilder builder)
    {
        services.AddDbContext<ReadWriteContext>(options =>
        {
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IDepartmentsRwRepository, DepartmentsReadWriteRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-departments-writes",
            tags: ["db", "sql", "departments", "writes"]);
    }
}
