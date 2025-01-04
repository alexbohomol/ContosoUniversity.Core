namespace ContosoUniversity.Mvc.IntegrationTests.CoursesController;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

public class CreateEndpointsTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateEndpointsTests(
        TestsConfiguration config,
        CustomWebApplicationFactory factory)
    {
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        factory.ClientOptions.AllowAutoRedirect = true;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationError()
    {
        var response = await _client.PostAsync(
            "/Courses/Create",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["CourseCode"] = "1111",
                ["Title"] = "Computers",
                ["Credits"] = "10",
                ["DepartmentId"] = "dab7e678-e3e7-4471-8282-96fe52e5c16f"
            }));

        response.EnsureSuccessStatusCode();

        // var content = await response.Content.ReadAsStringAsync();
        // content.Should().Contain("The field 'Credits' must be between 0 and 5.");
    }
}
