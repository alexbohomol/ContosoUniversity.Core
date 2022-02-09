namespace ContosoUniversity.Data.Courses.Writes;

using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaWrites(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadWriteContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<ICoursesRwRepository, ReadWriteRepository>();
    }

    public static async Task<bool> EnsureCoursesSchemaIsAvailable(this IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ReadWriteContext>();

        return await context.Database.CanConnectAsync();
    }
}