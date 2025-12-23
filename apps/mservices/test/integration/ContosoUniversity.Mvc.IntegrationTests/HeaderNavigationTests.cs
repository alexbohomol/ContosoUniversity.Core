namespace ContosoUniversity.Mvc.IntegrationTests;

using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

[Collection(nameof(SharedTestCollection))]
public class HeaderNavigationTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<WebApplicationFactory<IAssemblyMarker>>
{
    private readonly HttpClient _httpClient;

    public HeaderNavigationTests(
        TestsConfiguration config,
        WebApplicationFactory<IAssemblyMarker> factory)
    {
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home")]
    [InlineData("/Home/About")]
    [InlineData("/Students", Skip = "To be fixed with POST-mock")]
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
