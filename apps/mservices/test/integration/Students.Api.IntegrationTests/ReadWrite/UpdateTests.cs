namespace Students.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using IntegrationTests;

using Models;

public class UpdateTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<MsSqlContext>
{
    private readonly HttpClient _httpClient;

    public UpdateTests(
        TestsConfiguration config,
        DefaultApplicationFactory<IAssemblyMarker> factory,
        MsSqlContext msSqlContext)
    {
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ValidUpdateRequest_ReturnsOk()
    {
        // Arrange
        (_, Uri location) = await _httpClient.CreateStudent(Requests.Create.Student.Valid);
        var request = Requests.Update.Student.Valid;

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, request, default);
        var updatedCourse = await response.Content.ReadFromJsonAsync<UpdateStudentResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedCourse.Should().BeEquivalentTo(request);
    }
}
