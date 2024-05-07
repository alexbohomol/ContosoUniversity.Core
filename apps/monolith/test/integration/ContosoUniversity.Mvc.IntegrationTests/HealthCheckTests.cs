namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Data;

using FluentAssertions;

using HealthChecks.UI.Core;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Xunit;

public class HealthCheckTests// : SystemTest
{
    // private readonly HttpClient _httpClient;

    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();

    public HealthCheckTests()
    {
        // factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        // _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        var factory = new WebApplicationFactory<Program>();
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        var client = factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("health");

        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(HealthChecksJsonOptions);

        report.ShouldBeSuccessful();
    }

    [Fact]
    public async Task HealthCheck_ReturnsFailed()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(collection =>
            {
                var resolver = collection.BuildServiceProvider().GetService<IConnectionResolver>();
                collection.RemoveAll<IConnectionResolver>();
                collection.AddSingleton<IConnectionResolver>(_ => new TestConnectionResolver(resolver)
                {
                    Configure = builder1 =>
                    {
                        builder1.DataSource = "wronghost,1234";
                        builder1.ConnectTimeout = 5;
                    }
                });
            });
        });
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        var client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("health");

        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(HealthChecksJsonOptions);

        // report.ShouldBeSuccessful();
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

    private class TestConnectionResolver(IConnectionResolver resolver) : IConnectionResolver
    {
        public SqlConnectionStringBuilder CreateFor(string connectionStringName)
        {
            var builder = resolver.CreateFor(connectionStringName);
            Configure(builder);
            return builder;
        }

        public Action<SqlConnectionStringBuilder> Configure { get; set; } = _ => { };
    }
}
