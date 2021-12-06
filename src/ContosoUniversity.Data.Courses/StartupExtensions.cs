namespace ContosoUniversity.Data.Courses;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Reads;

using Writes;

public static class StartupExtensions
{
    public static void AddCoursesSchema(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadWriteContext>(options => { options.UseSqlServer(connectionString); });
        services.AddDbContext<ReadOnlyContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();
        services.AddScoped<ICoursesRwRepository, ReadWriteRepository>();
    }
}