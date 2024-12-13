namespace Students.Data.Reads;

using ContosoUniversity.Data;

using Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaReads(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var connectionResolver = provider.GetService<IConnectionResolver>();
            var connStringBuilder = connectionResolver.CreateFor("Students-RO");

            var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyContext>();
            optionsBuilder.UseSqlServer(connStringBuilder.ConnectionString);

            return new ReadOnlyContext(optionsBuilder.Options);
        });

        services.AddScoped<IStudentsRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-students-reads",
            tags: ["db", "sql", "students", "reads"]);
    }
}
