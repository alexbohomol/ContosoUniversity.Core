namespace Students.Api.IntegrationTests.HealthCheck;

using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using HealthChecks.UI.Core;

using IntegrationTesting.SharedKernel;

using Xunit;

public class InfraExistsTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public InfraExistsTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("/health/readiness")]
    [InlineData("/health/liveness")]
    public async Task Health_ReturnsHealthy(string healthUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(healthUrl);

        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(JsonSerializerOptionsBuilder.HealthChecks);

        report.ShouldBeHealthy(
            expectedCheckNames:
            [
                "sql-students-reads",
                "sql-students-writes"
            ],
            expectedTags:
            [
                "db",
                "sql",
                "students",
                "reads",
                "writes"
            ]);
    }
}
