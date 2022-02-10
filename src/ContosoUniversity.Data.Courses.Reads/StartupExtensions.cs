namespace ContosoUniversity.Data.Courses.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaReads(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadOnlyContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Courses-RO"));
        });

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();
    }
}