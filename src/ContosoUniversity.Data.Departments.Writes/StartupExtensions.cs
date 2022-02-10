namespace ContosoUniversity.Data.Departments.Writes;

using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDepartmentsSchemaWrites(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadWriteContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Departments-RW"));
        });

        services.AddScoped<IDepartmentsRwRepository, DepartmentsReadWriteRepository>();
        services.AddScoped<IInstructorsRwRepository, InstructorsReadWriteRepository>();
    }

    public static async Task<bool> EnsureDepartmentsSchemaIsAvailable(this IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ReadWriteContext>();

        return await context.Database.CanConnectAsync();
    }
}