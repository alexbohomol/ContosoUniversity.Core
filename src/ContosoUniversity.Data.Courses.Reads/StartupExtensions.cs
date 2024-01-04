namespace ContosoUniversity.Data.Courses.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaReads(this IServiceCollection services, SqlConnectionStringBuilder builder)
    {
        services.AddDbContext<ReadOnlyContext>(options =>
        {
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<ICoursesRoRepository, ReadOnlyRepository>();
    }
}
