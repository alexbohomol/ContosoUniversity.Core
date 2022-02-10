namespace ContosoUniversity.Data.Students.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaReads(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadOnlyContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Students-RO"));
        });

        services.AddScoped<IStudentsRoRepository, ReadOnlyRepository>();
    }
}