namespace ContosoUniversity.Data.Courses.Reads;

using Application.Contracts.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaReads(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadOnlyContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();
    }
}