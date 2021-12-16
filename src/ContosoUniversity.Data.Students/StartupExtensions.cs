namespace ContosoUniversity.Data.Students;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchema(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StudentsContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IStudentsRepository, StudentsRepository>();
        services.AddScoped<IStudentsRoRepository, StudentsRepository>();
        services.AddScoped<IStudentsRwRepository, StudentsRepository>();
    }
}