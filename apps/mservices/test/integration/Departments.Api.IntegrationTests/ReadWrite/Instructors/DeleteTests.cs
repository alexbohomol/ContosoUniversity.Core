namespace Departments.Api.IntegrationTests.ReadWrite.Instructors;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

public class DeleteTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<MsSqlContext>
{
    private readonly HttpClient _httpClient;

    public DeleteTests(
        TestsConfiguration config,
        DefaultApplicationFactory<IAssemblyMarker> factory,
        MsSqlContext msSqlContext)
    {
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task StudentExists_ReturnsNoContent()
    {
        // Arrange
        (_, Uri location) = await _httpClient.CreateInstructor(Requests.Create.Instructor.Valid);

        // Act
        var response = await _httpClient.DeleteAsync(location, default);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        content.Should().BeEmpty();
    }
}
