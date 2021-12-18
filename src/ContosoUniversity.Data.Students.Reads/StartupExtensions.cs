namespace ContosoUniversity.Data.Students.Reads;

using Application.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaReads(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadOnlyContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IStudentsRoRepository, ReadOnlyRepository>();
    }
}