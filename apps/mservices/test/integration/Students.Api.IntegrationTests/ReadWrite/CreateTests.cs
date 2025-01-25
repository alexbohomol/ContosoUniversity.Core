namespace Students.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using IntegrationTests;

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
        var request = Requests.CreateStudent.Valid;

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
