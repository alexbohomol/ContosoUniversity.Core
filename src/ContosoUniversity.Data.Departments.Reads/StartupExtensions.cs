namespace ContosoUniversity.Data.Departments.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaReads(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadOnlyContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Departments-RO"));
        });

        services.AddScoped<IDepartmentsRoRepository, DepartmentsReadOnlyRepository>();
        services.AddScoped<IInstructorsRoRepository, InstructorsReadOnlyRepository>();
    }
}