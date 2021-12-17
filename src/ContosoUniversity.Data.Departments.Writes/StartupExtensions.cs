namespace ContosoUniversity.Data.Departments.Writes;

using Application;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaWrites(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadWriteContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IDepartmentsRwRepository, DepartmentsReadWriteRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsReadWriteRepository>();
    }
}