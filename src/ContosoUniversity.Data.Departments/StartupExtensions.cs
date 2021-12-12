namespace ContosoUniversity.Data.Departments;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchema(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DepartmentsContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IDepartmentsRoRepository, DepartmentsRoRepository>();
        services.AddScoped<IDepartmentsRwRepository, DepartmentsRwRepository>();

        services.AddScoped<IInstructorsRepository, InstructorsRepository>();
        services.AddScoped<IInstructorsRoRepository, InstructorsRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsRepository>();
    }
}