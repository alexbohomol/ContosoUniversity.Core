namespace Students.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using IntegrationTests;

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
        (CreateStudentResponse created, Uri location) = await _httpClient.CreateStudent(Requests.CreateStudent.Valid);
        var request = Requests.UpdateStudent.Valid with
        {
            ExternalId = created.ExternalId
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, request, default);
        var updatedCourse = await response.Content.ReadFromJsonAsync<EditStudentResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedCourse.Should().BeEquivalentTo(request);
    }
}
