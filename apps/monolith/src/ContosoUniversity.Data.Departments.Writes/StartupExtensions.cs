namespace ContosoUniversity.Data.Departments.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaWrites(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var connectionResolver = provider.GetService<IConnectionResolver>();
            var connStringBuilder = connectionResolver.CreateFor("Departments-RW");

            var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
            optionsBuilder.UseSqlServer(connStringBuilder.ConnectionString);

            return new ReadWriteContext(optionsBuilder.Options);
        });

        services.AddScoped<IDepartmentsRwRepository, DepartmentsReadWriteRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-departments-writes",
            tags: ["db", "sql", "departments", "writes"]);
    }
}
