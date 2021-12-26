namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

public class HeaderNavigationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public HeaderNavigationTests(WebApplicationFactory<Program> factory)
    {
        factory.ClientOptions.BaseAddress = new Uri("http://localhost:5000");
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