namespace ContosoUniversity.Data.Connection;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

internal class SqlConnectionStringBuilderSetup(IConfiguration configuration)
    : IConfigureOptions<SqlConnectionStringBuilder>
{
    public void Configure(SqlConnectionStringBuilder options)
    {
        configuration
            .GetSection("SqlConnectionStringBuilder")
            .Bind(options);
    }
}
