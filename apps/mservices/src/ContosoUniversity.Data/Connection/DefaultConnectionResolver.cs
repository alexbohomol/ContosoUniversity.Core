namespace ContosoUniversity.Data.Connection;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

internal class DefaultConnectionResolver(IConfiguration configuration)
    : IConnectionResolver
{
    public SqlConnectionStringBuilder CreateFor(string connectionStringName)
    {
        var defaults = configuration
            .GetSection(nameof(SqlConnectionStringBuilder))
            .Get<SqlConnectionStringBuilder>();

        return new(configuration.GetConnectionString(connectionStringName))
        {
            DataSource = defaults.DataSource,
            InitialCatalog = defaults.InitialCatalog,
            MultipleActiveResultSets = defaults.MultipleActiveResultSets,
            TrustServerCertificate = defaults.TrustServerCertificate
        };
    }
}
