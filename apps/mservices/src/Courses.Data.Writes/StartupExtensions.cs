namespace Courses.Data.Writes;

using ContosoUniversity.Data;

using Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddCoursesSchemaWrites(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var connectionResolver = provider.GetService<IConnectionResolver>();
            var connStringBuilder = connectionResolver.CreateFor("Courses-RW");

            var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
            optionsBuilder.UseSqlServer(connStringBuilder.ConnectionString);

            return new ReadWriteContext(optionsBuilder.Options);
        });

        services.AddScoped<ICoursesRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-courses-writes",
            tags: ["db", "sql", "courses", "writes"]);
    }
}
