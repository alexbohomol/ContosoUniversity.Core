namespace Students.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using IntegrationTests;

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
        (_, Uri location) = await _httpClient.CreateStudent(Requests.Create.Student.Valid);

        // Act
        var response = await _httpClient.DeleteAsync(location, default);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        content.Should().BeEmpty();
    }
}
