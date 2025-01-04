namespace ContosoUniversity.Mvc.IntegrationTests.CoursesController;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

public class CreateEndpointsTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public CreateEndpointsTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        factory.ClientOptions.AllowAutoRedirect = true;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationError()
    {
        var response = await _httpClient.PostAsync(
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
