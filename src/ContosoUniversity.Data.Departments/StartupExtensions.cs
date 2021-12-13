namespace ContosoUniversity.Data.Departments;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Reads;

using Writes;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaReads(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadOnlyContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IDepartmentsRoRepository, DepartmentsRoRepository>();
        services.AddScoped<IInstructorsRoRepository, InstructorsRoRepository>();
    }

    public static void AddDepartmentsSchemaWrites(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadWriteContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IDepartmentsRwRepository, DepartmentsRwRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsRwRepository>();
    }
}