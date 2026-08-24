namespace ContosoUniversity.Data.Students.Writes;

using Application.Contracts.Repositories.Writes;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddStudentsSchemaWrites(this IServiceCollection services)
    {
        services.AddDbContext<ReadWriteContext>((provider, options) =>
        {
            var config = provider.GetService<IOptionsMonitor<SqlConnectionStringBuilder>>();
            SqlConnectionStringBuilder builder = config.Get("Students-RW");
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IStudentsRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-students-writes",
            tags: ["db", "sql", "students", "writes"]);
    }
}
