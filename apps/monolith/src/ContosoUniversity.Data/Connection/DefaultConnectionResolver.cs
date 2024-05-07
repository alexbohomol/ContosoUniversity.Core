namespace ContosoUniversity.Data.Connection;

using System;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

internal class DefaultConnectionResolver(IConfiguration configuration) : IConnectionResolver
{
    public SqlConnectionStringBuilder CreateFor(string connectionStringName)
    {
        var defaults = configuration
            .GetSection(nameof(SqlConnectionStringBuilder))
            .Get<SqlConnectionStringBuilder>();

        return new(configuration.GetConnectionString(connectionStringName))
        {
            DataSource = Environment.GetEnvironmentVariable("CONTOSO_DB_HOST") ?? "localhost,1433",
            InitialCatalog = defaults.InitialCatalog,
            MultipleActiveResultSets = defaults.MultipleActiveResultSets,
            TrustServerCertificate = defaults.TrustServerCertificate
        };
    }
}
