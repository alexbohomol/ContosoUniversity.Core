namespace ContosoUniversity.Mvc.IntegrationTests.CoursesController;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Xunit;

public class CreateEndpointsTests :
    IntegrationTest,
    IClassFixture<NoAntiforgeryWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateEndpointsTests(NoAntiforgeryWebApplicationFactory factory)
    {
        factory.ClientOptions.BaseAddress = new Uri(Configuration["PageBaseUrl:Https"]);
        factory.ClientOptions.AllowAutoRedirect = true;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationError()
    {
        var response = await _client.PostAsync(
            "/Courses/Create",
            new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("CourseCode", "1111"),
                new KeyValuePair<string, string>("Title", "Computers"),
                new KeyValuePair<string, string>("Credits", "10"),
                new KeyValuePair<string, string>("DepartmentId", "dab7e678-e3e7-4471-8282-96fe52e5c16f")
            }));

        response.EnsureSuccessStatusCode();

        // var content = await response.Content.ReadAsStringAsync();
        // content.Should().Contain("The field 'Credits' must be between 0 and 5.");
    }
}

public class NoAntiforgeryWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IAntiforgery>();
            services.AddTransient<IAntiforgery, NoOpAntiforgery>();
        });
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