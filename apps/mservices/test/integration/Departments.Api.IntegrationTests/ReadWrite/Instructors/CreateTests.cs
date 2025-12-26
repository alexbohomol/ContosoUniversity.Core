namespace Departments.Api.IntegrationTests.ReadWrite.Instructors;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using Models;

public class CreateTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<MsSqlContext>
{
    private readonly HttpClient _httpClient;

    public CreateTests(
        TestsConfiguration config,
        DefaultApplicationFactory<IAssemblyMarker> factory,
        MsSqlContext msSqlContext)
    {
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ValidCreateRequest_ReturnsCreated()
    {
        // Arrange
        var request = Requests.Create.Instructor.Valid;

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/instructors", request, default);
        var createdInstructor = await response.Content.ReadFromJsonAsync<CreateInstructorResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/api/instructors/{createdInstructor.ExternalId}");
        createdInstructor.Should().BeEquivalentTo(request);
    }
}
