namespace ContosoUniversity.Mvc.IntegrationTests;

using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

public class HeaderNavigationTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public HeaderNavigationTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
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
