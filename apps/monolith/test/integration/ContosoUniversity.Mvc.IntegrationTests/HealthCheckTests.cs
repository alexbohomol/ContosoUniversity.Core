namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using FluentAssertions;

using HealthChecks.UI.Core;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

public class HealthCheckTests : SystemTest
{
    private readonly HttpClient _httpClient;

    private static readonly string[] CheckNames =
    [
        "sql-courses-reads",
        "sql-courses-writes",
        "sql-students-reads",
        "sql-students-writes",
        "sql-departments-reads",
        "sql-departments-writes"
    ];

    private static readonly string[] Tags =
    [
        "db",
        "sql",
        "courses",
        "students",
        "departments",
        "reads",
        "writes"
    ];

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("health");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(HealthChecksJsonOptions);

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        report.Should().NotBeNull();
        report.Status.Should().Be(UIHealthStatus.Healthy);
        // report.TotalDuration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        report.Entries.Should().NotBeEmpty();
        report.Entries.Count.Should().Be(6);
        report.Entries.Keys.Should().BeEquivalentTo(CheckNames);
        report.Entries.Values.SelectMany(x => x.Tags).Distinct().Should().BeEquivalentTo(Tags);

        foreach (var entry in report.Entries.Values)
        {
            entry.Data.Should().BeEmpty();
            entry.Status.Should().Be(UIHealthStatus.Healthy);
            // entry.Duration.Should().BeLessThan(TimeSpan.FromSeconds(1));
            entry.Tags?.Should().NotBeNull();
            entry.Tags?.Count().Should().Be(4);
        }
    }

    private static JsonSerializerOptions HealthChecksJsonOptions
    {
        get
        {
            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }
    }
}
