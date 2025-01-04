namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

using Microsoft.Data.SqlClient;

using Testcontainers.MsSql;

using Xunit;

public class InfrastructureContext : IAsyncLifetime
{
    private const string RelativePath = "../../../../../../../../database";

    private MsSqlContainer _msSqlContainer;
    private INetwork _network;
    private IContainer _migrator;

    private static string GetFullPath(string relativePath) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                relativePath));

    public async Task InitializeAsync()
    {
        _network = new NetworkBuilder()
            .WithName($"cunetwork-{Guid.NewGuid()}")
            .Build();

        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .WithNetwork(_network)
            .WithHostname("db")
            .WithWorkingDirectory("/scripts")
            .WithPassword("<YourStrong!Passw0rd>")
            .WithWaitStrategy(
                Wait
                    .ForUnixContainer()
                    .UntilPortIsAvailable(1433)
                    .UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -Q \"SELECT 1\" -C"))
            .Build();

        _migrator = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql-tools")
            .WithNetwork(_network)
            .DependsOn(_msSqlContainer)
            .WithBindMount(GetFullPath(RelativePath), "/scripts", AccessMode.ReadOnly)
            .WithWorkingDirectory("/scripts")
            .WithEntrypoint(
                "/opt/mssql-tools/bin/sqlcmd",
                "-S", "db",
                "-U", "sa",
                "-P", "<YourStrong!Passw0rd>",
                "-i", "db-init.sql")
            .Build();

        await _msSqlContainer.StartAsync();
        await _migrator.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        await _migrator.DisposeAsync();
        await _network.DisposeAsync();
    }

    public string MsSqlDataSource
    {
        get
        {
            // EnsureInfrastructureState();
            var connString = _msSqlContainer.GetConnectionString();
            var parsed = new SqlConnectionStringBuilder(connString);
            return parsed.DataSource;
        }
    }

    private void EnsureInfrastructureState()
    {
        if (_msSqlContainer.State == TestcontainersStates.Running &&
            _msSqlContainer.Health == TestcontainersHealthStatus.Healthy &&
            _migrator.State == TestcontainersStates.Exited)
        {
            return;
        }

        throw new InvalidOperationException("Infrastructure context is not yet ready.");
    }
}
