namespace ContosoUniversity.Data.Students.Writes;

using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaWrites(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadWriteContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Students-RW"));
        });

        services.AddScoped<IStudentsRwRepository, ReadWriteRepository>();
    }

    public static async Task<bool> EnsureStudentsSchemaIsAvailable(this IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ReadWriteContext>();

        return await context.Database.CanConnectAsync();
    }
}