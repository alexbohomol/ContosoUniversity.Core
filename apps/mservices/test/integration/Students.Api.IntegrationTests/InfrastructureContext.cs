namespace Students.Api.IntegrationTests;

using System;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

using Microsoft.Data.SqlClient;

using Testcontainers.MsSql;

using Xunit;

public class InfrastructureContext : IAsyncLifetime
{
    private const string DatabaseDirectory = "../../../../../../../../database";

    private static string GetFullPathTo(string relativePath) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                relativePath));

    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server")
        .WithBindMount(GetFullPathTo(DatabaseDirectory), "/scripts", AccessMode.ReadOnly)
        .WithWorkingDirectory("/scripts")
        // .WithPortBinding(1433, 1433)
        // .WithPassword("<YourStrong!Passw0rd>")
        .WithWaitStrategy(
            Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(1433)
                .UntilCommandIsCompleted(
                [
                    "/opt/mssql-tools18/bin/sqlcmd",
                    "-Q", "SELECT 1;",
                    "-C"  // Trust the self-signed certificate
                ]))
        .Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var execResult = await _msSqlContainer.ExecAsync(
        [
            "/opt/mssql-tools18/bin/sqlcmd",
            "-i", "db-init.sql",
            "-C"  // Trust the self-signed certificate
        ]);

        if (execResult is not { Stderr: "", ExitCode: 0 })
        {
            throw new InvalidOperationException(
                $"Database initialization failed: {execResult.Stderr}");
        }
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }

    public string MsSqlDataSource
    {
        get
        {
            var connString = _msSqlContainer.GetConnectionString();
            var parsed = new SqlConnectionStringBuilder(connString);
            return parsed.DataSource;
        }
    }
}
