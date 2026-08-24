namespace ContosoUniversity.Data.Departments.Reads;

using Application.Contracts.Repositories.Reads;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaReads(this IServiceCollection services)
    {
        services.AddDbContext<ReadOnlyContext>((provider, options) =>
        {
            var config = provider.GetService<IOptionsMonitor<SqlConnectionStringBuilder>>();
            SqlConnectionStringBuilder builder = config.Get("Departments-RO");
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IDepartmentsRoRepository, DepartmentsReadOnlyRepository>();
        services.AddScoped<IInstructorsRoRepository, InstructorsReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-departments-reads",
            tags: ["db", "sql", "departments", "reads"]);
    }
}
