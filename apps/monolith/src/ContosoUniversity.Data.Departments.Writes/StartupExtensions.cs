namespace ContosoUniversity.Data.Departments.Writes;

using Application.Contracts.Repositories.Writes;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaWrites(this IServiceCollection services)
    {
        services.AddDbContext<ReadWriteContext>((provider, options) =>
        {
            var config = provider.GetService<IOptionsMonitor<SqlConnectionStringBuilder>>();
            SqlConnectionStringBuilder builder = config.Get("Departments-RW");
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IDepartmentsRwRepository, DepartmentsReadWriteRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-departments-writes",
            tags: ["db", "sql", "departments", "writes"]);
    }
}
