namespace Departments.Data.Reads;

using ContosoUniversity.Data;

using Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaReads(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var connectionResolver = provider.GetService<IConnectionResolver>();
            var connStringBuilder = connectionResolver.CreateFor("Departments-RO");

            var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyContext>();
            optionsBuilder.UseSqlServer(connStringBuilder.ConnectionString);

            return new ReadOnlyContext(optionsBuilder.Options);
        });

        services.AddScoped<IDepartmentsRoRepository, DepartmentsReadOnlyRepository>();
        services.AddScoped<IInstructorsRoRepository, InstructorsReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-departments-reads",
            tags: ["db", "sql", "departments", "reads"]);
    }
}
