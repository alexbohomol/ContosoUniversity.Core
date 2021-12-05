namespace ContosoUniversity.Data.Courses;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchema(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CoursesContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<ICoursesRoRepository, CoursesRepository>();
        services.AddScoped<ICoursesRwRepository, CoursesRepository>();
    }
}