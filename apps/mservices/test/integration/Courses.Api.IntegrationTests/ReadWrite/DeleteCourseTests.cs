namespace Courses.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

public class DeleteCourseTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public DeleteCourseTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CourseExists_ReturnsNoContent()
    {
        // Arrange
        (_, Uri location) = await _httpClient.CreateCourse(Requests.CreateCourse.Valid);

        // Act
        var response = await _httpClient.DeleteAsync(location, default);

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
