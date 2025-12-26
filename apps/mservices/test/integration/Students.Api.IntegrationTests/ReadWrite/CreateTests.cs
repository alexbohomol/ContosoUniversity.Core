namespace Students.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using IntegrationTests;

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
        var request = Requests.Create.Student.Valid;

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/students", request, default);
        var createdCourse = await response.Content.ReadFromJsonAsync<CreateStudentResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/api/students/{createdCourse.ExternalId}");
        createdCourse.Should().BeEquivalentTo(request);
    }
}
