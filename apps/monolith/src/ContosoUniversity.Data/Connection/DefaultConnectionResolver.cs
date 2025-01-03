namespace ContosoUniversity.Data.Connection;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

internal class DefaultConnectionResolver(
    IConfiguration configuration,
    IOptions<SqlConnectionStringBuilder> options)
    : IConnectionResolver
{
    public SqlConnectionStringBuilder CreateFor(string connectionStringName)
    {
        SqlConnectionStringBuilder defaults = options.Value;

        return new(configuration.GetConnectionString(connectionStringName))
        {
            DataSource = defaults.DataSource,
            InitialCatalog = defaults.InitialCatalog,
            MultipleActiveResultSets = defaults.MultipleActiveResultSets,
            TrustServerCertificate = defaults.TrustServerCertificate
        };
    }
}
