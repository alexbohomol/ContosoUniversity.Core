namespace ContosoUniversity.Data.Students.Reads;

using Application.Contracts.Repositories.ReadOnly;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaReads(this IServiceCollection services, SqlConnectionStringBuilder builder)
    {
        services.AddDbContext<ReadOnlyContext>(options =>
        {
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IStudentsRoRepository, ReadOnlyRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadOnlyContext>(
            name: "sql-students-reads",
            tags: ["db", "sql", "students", "reads"]);
    }
}
