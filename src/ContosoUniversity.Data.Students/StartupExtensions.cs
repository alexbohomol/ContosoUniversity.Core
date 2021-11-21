namespace ContosoUniversity.Data.Students;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsDataLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StudentsContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IStudentsRepository, StudentsRepository>();
    }
}