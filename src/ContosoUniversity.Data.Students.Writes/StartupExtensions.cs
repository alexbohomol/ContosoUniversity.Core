namespace ContosoUniversity.Data.Students.Writes;

using Application.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaWrites(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadWriteContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IStudentsRwRepository, ReadWriteRepository>();
    }
}