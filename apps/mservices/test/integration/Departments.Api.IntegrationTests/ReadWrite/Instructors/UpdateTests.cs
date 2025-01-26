namespace Departments.Api.IntegrationTests.ReadWrite.Instructors;

using System.Net;

using FluentAssertions;

using Models;

public class UpdateTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>
{
    private readonly HttpClient _httpClient;

    public UpdateTests(
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
        (_, Uri location) = await _httpClient.CreateInstructor(Requests.CreateInstructor.Valid);
        var request = Requests.UpdateInstructor.Valid;

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, request, default);
        var updatedInstructor = await response.Content.ReadFromJsonAsync<UpdateInstructorResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // updatedInstructor.Should().BeEquivalentTo(request);
        // updatedInstructor.ExternalId.Should().Be(request.ExternalId);
        updatedInstructor.LastName.Should().Be(request.LastName);
        updatedInstructor.FirstName.Should().Be(request.FirstName);
        updatedInstructor.HireDate.Should().Be(request.HireDate);
        updatedInstructor.SelectedCourses.Should().BeEquivalentTo(request.SelectedCourses);
    }
}
