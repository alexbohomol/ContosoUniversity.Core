namespace ContosoUniversity.Data;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Options;

public static class StartupExtensions
{
    public static void AddDataInfrastructure(this IServiceCollection services)
    {
        services.ConfigureOptions<SqlConnectionStringBuilderSetup>();
    }

    public static void AddDbContext<TDbContext>(
        this IServiceCollection services,
        string connectionStringName)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((provider, options) =>
        {
            var connectionString = provider
                .GetRequiredService<IOptionsMonitor<SqlConnectionStringBuilder>>()
                .Get(connectionStringName)
                .ConnectionString;

            options.UseSqlServer(connectionString);
        });
    }
}
