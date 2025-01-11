namespace ContosoUniversity.Data;

using Connection;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class StartupExtensions
{
    public static void AddDataInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionResolver, DefaultConnectionResolver>();

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
                .GetRequiredService<IConfiguration>()
                .GetConnectionString(connectionStringName);

            var defaults = provider
                .GetRequiredService<IOptions<SqlConnectionStringBuilder>>()
                .Value;

            connectionString = connectionString.WrapWithDefaults(defaults);

            options.UseSqlServer(connectionString);
        });
    }

    private static string WrapWithDefaults(
        this string connectionString,
        SqlConnectionStringBuilder defaults)
        => new SqlConnectionStringBuilder(connectionString)
        {
            DataSource = defaults.DataSource,
            InitialCatalog = defaults.InitialCatalog,
            MultipleActiveResultSets = defaults.MultipleActiveResultSets,
            TrustServerCertificate = defaults.TrustServerCertificate
        }.ConnectionString;
}
