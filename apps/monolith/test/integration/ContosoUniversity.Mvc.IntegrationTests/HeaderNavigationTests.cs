namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

using FluentAssertions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

using Testcontainers.MsSql;

using Xunit;

public class HeaderNavigationTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public HeaderNavigationTests(
        TestsConfiguration config,
        CustomWebApplicationFactory factory)
    {
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home")]
    [InlineData("/Home/About")]
    [InlineData("/Students")]
    [InlineData("/Courses")]
    [InlineData("/Instructors")]
    [InlineData("/Departments")]
    public async Task HeaderMenu_Smoke_ReturnsOk(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("text/html; charset=utf-8");
    }
}

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
            // .WithPortBinding(1433, 1433)
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

        string connectionString = _msSqlContainer.GetConnectionString();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
        _dataSource = builder.DataSource;
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        await _migrator.DisposeAsync();
        await _network.DisposeAsync();
    }
}
