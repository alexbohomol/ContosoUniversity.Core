namespace ContosoUniversity.Data.Students.Writes;

using Application.Contracts.Repositories.ReadWrite;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddStudentsSchemaWrites(this IServiceCollection services, SqlConnectionStringBuilder builder)
    {
        services.AddDbContext<ReadWriteContext>(options =>
        {
            options.UseSqlServer(builder.ConnectionString);
        });

        services.AddScoped<IStudentsRwRepository, ReadWriteRepository>();

        services.AddHealthChecks().AddDbContextCheck<ReadWriteContext>(
            name: "sql-students-writes",
            tags: ["db", "sql", "students", "writes"]);
    }
}
