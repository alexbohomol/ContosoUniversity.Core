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
        (_, Uri location) = await CreateCourse();
        var updateRequest = new UpdateCourseRequest("Quantum Computing", 3, Guid.NewGuid());

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, updateRequest, default);
        var updatedCourse = await response.Content.ReadFromJsonAsync<UpdateCourseResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedCourse.Should().BeEquivalentTo(updateRequest);
    }

    private async Task<(CreateCourseResponse created, Uri Location)> CreateCourse()
    {
        var courseRequest = new CreateCourseRequest(1234, "Computers", 5, Guid.NewGuid());
        var response = await _httpClient.PostAsJsonAsync("/api/courses", courseRequest, default);
        var createdCourse = await response.Content.ReadFromJsonAsync<CreateCourseResponse>();
        return (createdCourse, response.Headers.Location);
    }
}
