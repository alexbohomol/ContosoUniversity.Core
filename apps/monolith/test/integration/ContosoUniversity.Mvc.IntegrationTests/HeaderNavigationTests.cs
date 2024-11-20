namespace ContosoUniversity.Mvc.IntegrationTests;

using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

[Collection(nameof(SharedTestCollection))]
public class HeaderNavigationTests(SharedTestContext context)
{
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
        HttpResponseMessage response = await context.Client.GetAsync(url);

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("text/html; charset=utf-8");
    }
}
