namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Testcontainers.MsSql;

using Xunit;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private MsSqlContainer _msSqlContainer;
    private INetwork _network;
    private IContainer _migrator;
    private const string RelativePath = "../../../../../../../../database";
    private string _dataSource;

    private static string GetFullPath(string relativePath) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                relativePath));

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            if (string.IsNullOrWhiteSpace(_dataSource))
            {
                throw new InvalidOperationException("Wrong test life-cycle!");
            }

            services.Configure<SqlConnectionStringBuilder>(options =>
            {
                options.DataSource = _dataSource;
            });

            services.RemoveAll<IAntiforgery>();
            services.AddTransient<IAntiforgery, NoOpAntiforgery>();
        });
    }

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

        var connectionString = _msSqlContainer.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(connectionString);
        _dataSource = builder.DataSource;
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        await _migrator.DisposeAsync();
        await _network.DisposeAsync();
    }
}

file class NoOpAntiforgery : IAntiforgery
{
    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext) =>
        new("test", "test", "test", "test");

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext) =>
        new("test", "test", "test", "test");

    public Task<bool> IsRequestValidAsync(HttpContext httpContext) =>
        Task.FromResult(true);

    public void SetCookieTokenAndHeader(HttpContext httpContext) { }

    public Task ValidateRequestAsync(HttpContext httpContext) =>
        Task.CompletedTask;
}
