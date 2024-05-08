namespace ContosoUniversity.Data.Students.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaWrites(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var connectionResolver = provider.GetService<IConnectionResolver>();
            var connStringBuilder = connectionResolver.CreateFor("Students-RW");

            var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
            optionsBuilder.UseSqlServer(connStringBuilder.ConnectionString);

            return new ReadWriteContext(optionsBuilder.Options);
        });

        services.AddScoped<IStudentsRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-students-writes",
            tags: ["db", "sql", "students", "writes"]);
    }
}
