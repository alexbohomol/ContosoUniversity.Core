namespace Courses.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using Models;

public class CreateCourseTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public CreateCourseTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ValidCreateCourseRequest_ReturnsOk()
    {
        // Arrange
        var courseRequest = new CreateCourseRequest(1234, "Computers", 5, Guid.NewGuid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/courses", courseRequest, CancellationToken.None);
        var createdCourse = await response.Content.ReadFromJsonAsync<CreateCourseResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/api/courses/{createdCourse.ExternalId}");
        createdCourse.Should().BeEquivalentTo(courseRequest);
    }
}
