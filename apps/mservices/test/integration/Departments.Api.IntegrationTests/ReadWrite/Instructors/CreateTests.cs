namespace Departments.Api.IntegrationTests.ReadWrite.Instructors;

using System.Net;

using FluentAssertions;

using Models;

public class CreateTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public CreateTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext context)
    {
        factory.DataSourceSetterFunction = () => context.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ValidCreateRequest_ReturnsCreated()
    {
        // Arrange
        var request = Requests.CreateInstructor.Valid;

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/instructors", request, default);
        var createdInstructor = await response.Content.ReadFromJsonAsync<CreateInstructorResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/api/instructors/{createdInstructor.ExternalId}");
        // createdInstructor.Should().BeEquivalentTo(request);
        createdInstructor.LastName.Should().Be(request.LastName);
        createdInstructor.FirstName.Should().Be(request.FirstName);
        createdInstructor.HireDate.Should().Be(request.HireDate);
        createdInstructor.SelectedCourses.Should().BeEquivalentTo(request.SelectedCourses);
    }
}
