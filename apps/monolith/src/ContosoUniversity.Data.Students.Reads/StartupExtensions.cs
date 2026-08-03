namespace ContosoUniversity.Data.Students.Reads;

using Application.Contracts.Repositories.Reads;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddStudentsSchemaReads(this IServiceCollection services)
    {
        services.AddDbContext<ReadOnlyContext>((provider, options) =>
        {
            var config = provider.GetService<IOptionsMonitor<SqlConnectionStringBuilder>>();
            SqlConnectionStringBuilder builder = config.Get("Students-RO");
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IStudentsRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-students-reads",
            tags: ["db", "sql", "students", "reads"]);
    }
}
