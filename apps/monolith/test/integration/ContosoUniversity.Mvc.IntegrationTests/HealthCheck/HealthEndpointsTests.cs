namespace ContosoUniversity.Mvc.IntegrationTests.HealthCheck;

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

public class HealthEndpointsTests
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();

    [Theory]
    [InlineData("/health/readiness")]
    [InlineData("/health/liveness")]
    public async Task Health_ReturnsHealthy(string healthUrl)
    {
        var factory = new WebApplicationFactory<Program>();
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        var client = factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync(healthUrl);

        response.Should().BeSuccessful();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(HealthChecksJsonOptions);

        report.ShouldBeHealthy();
    }

    [Theory]
    [InlineData("/health/readiness")]
    [InlineData("/health/liveness")]
    public async Task Health_ReturnsUnhealthy(string healthUrl)
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(collection =>
            {
                var resolver = collection.BuildServiceProvider().GetService<IConnectionResolver>();
                collection.RemoveAll<IConnectionResolver>();
                collection.AddSingleton<IConnectionResolver>(
                    new DecoratingConnectionResolver(
                        resolver,
                        sqlBuilder =>
                        {
                            sqlBuilder.DataSource = "wrong host,1234";
                            sqlBuilder.ConnectTimeout = 5;
                        }));
            });
        });
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        var client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync(healthUrl);

        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json");

        var report = await response.Content.ReadFromJsonAsync<UIHealthReport>(HealthChecksJsonOptions);

        report.ShouldBeUnhealthy();
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

    private class DecoratingConnectionResolver(
        IConnectionResolver originalResolver,
        Action<SqlConnectionStringBuilder> decorate)
        : IConnectionResolver
    {
        public SqlConnectionStringBuilder CreateFor(string connectionStringName)
        {
            var builder = originalResolver.CreateFor(connectionStringName);
            decorate(builder);
            return builder;
        }
    }
}
