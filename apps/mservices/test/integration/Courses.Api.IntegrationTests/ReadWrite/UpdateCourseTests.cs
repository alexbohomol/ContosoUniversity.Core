namespace Courses.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using Models;

public class UpdateCourseTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public UpdateCourseTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ValidUpdateRequest_ReturnsOk()
    {
        // Arrange
        (_, Uri location) = await _httpClient.CreateCourse(Requests.CreateCourse.Valid);
        var request = Requests.UpdateCourse.Valid;

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, request, default);
        var updatedCourse = await response.Content.ReadFromJsonAsync<UpdateCourseResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedCourse.Should().BeEquivalentTo(request);
    }
}
