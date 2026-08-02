namespace ContosoUniversity.Data.Options;

using System;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

internal class SqlConnectionStringBuilderSetup(IConfiguration configuration)
    : IConfigureNamedOptions<SqlConnectionStringBuilder>
{
    private const string SectionName = "SqlConnectionStringBuilder";

    public void Configure(string name, SqlConnectionStringBuilder options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var rootSection = configuration.GetRequiredSection(SectionName);

        rootSection.GetRequiredSection("Defaults").Bind(options);

        var overrideSection = rootSection.GetRequiredSection($"Overrides:{name}");

        overrideSection.Bind(options);
    }

    public void Configure(SqlConnectionStringBuilder options)
        => Configure(Options.DefaultName, options);
}
