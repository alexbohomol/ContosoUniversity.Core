namespace Courses.Api.IntegrationTests.HealthCheck;

using System.Net;
using System.Threading.Tasks;

using Api;

using FluentAssertions;

using HealthChecks.UI.Core;

using IntegrationTesting.SharedKernel;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

public class NoInfraTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<WebApplicationFactory<IAssemblyMarker>>
{
    private readonly HttpClient _httpClient;

    public NoInfraTests(
        TestsConfiguration config,
        WebApplicationFactory<IAssemblyMarker> factory)
    {
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Theory(Skip = "Should elaborate later on how to perform and setup it")]
    [InlineData("/health/readiness")]
    [InlineData("/health/liveness")]
    public async Task Health_ReturnsUnhealthy(string healthUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(healthUrl);

        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(JsonSerializerOptionsBuilder.HealthChecks);

        report.ShouldBeUnhealthy(
            expectedCheckNames:
            [
                "sql-courses-reads",
                "sql-courses-writes"
            ],
            expectedTags:
            [
                "db",
                "sql",
                "courses",
                "reads",
                "writes"
            ]);
    }
}
