namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

using HealthChecks.UI.Core;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

public class HealthCheckTests : SystemTest
{
    private readonly HttpClient _httpClient;

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("health");

        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(HealthChecksJsonOptions);

        report.ShouldBeSuccessful();
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
