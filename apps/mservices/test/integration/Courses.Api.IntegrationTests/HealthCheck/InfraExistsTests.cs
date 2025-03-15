namespace Courses.Api.IntegrationTests.HealthCheck;

using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using HealthChecks.UI.Core;

using IntegrationTesting.SharedKernel;

using Xunit;

public class InfraExistsTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>,
    IClassFixture<RabbitMqContext>
{
    private readonly HttpClient _httpClient;

    public InfraExistsTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context,
        RabbitMqContext rabbitMqContext)
    {
        factory.RabbitMqConnectionSetterFunction = () => rabbitMqContext.ConnectionString;
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("/health/readiness")]
    [InlineData("/health/liveness")]
    public async Task Health_ReturnsHealthy(string healthUrl)
    {
        await Task.Delay(1000);
        HttpResponseMessage response = await _httpClient.GetAsync(healthUrl);

        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(JsonSerializerOptionsBuilder.HealthChecks);

        report.Should().NotBeNull();
        report.Status.Should().Be(UIHealthStatus.Healthy);
        report.Entries.Should().NotBeEmpty();
        report.Entries.Keys.Should().BeEquivalentTo([
            "sql-courses-reads",
            "sql-courses-writes",
            "masstransit-bus"
        ]);
        report.Entries["sql-courses-reads"].Status.Should().Be(UIHealthStatus.Healthy);
        report.Entries["sql-courses-reads"].Tags.Should().BeEquivalentTo([
            "db",
            "sql",
            "courses",
            "reads"
        ]);
        report.Entries["sql-courses-writes"].Status.Should().Be(UIHealthStatus.Healthy);
        report.Entries["sql-courses-writes"].Tags.Should().BeEquivalentTo([
            "db",
            "sql",
            "courses",
            "writes"
        ]);
        report.Entries["masstransit-bus"].Status.Should().Be(UIHealthStatus.Healthy);
        report.Entries["masstransit-bus"].Tags.Should().BeEquivalentTo([
            "ready",
            "masstransit"
        ]);
    }
}
