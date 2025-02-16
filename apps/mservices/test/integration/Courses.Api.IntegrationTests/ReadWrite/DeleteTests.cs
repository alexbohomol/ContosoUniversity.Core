namespace Courses.Api.IntegrationTests.ReadWrite;

using System.Net;

using FluentAssertions;

public class DeleteTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>,
    IClassFixture<RabbitMqContext>
{
    private readonly HttpClient _httpClient;

    public DeleteTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext msSqlContext,
        RabbitMqContext rabbitMqContext)
    {
        factory.RabbitMqConnectionSetterFunction = () => rabbitMqContext.GetConnectionString;
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CourseExists_ReturnsNoContent()
    {
        // Arrange
        (_, Uri location) = await _httpClient.CreateCourse(Requests.Create.Course.Valid);

        // Act
        var response = await _httpClient.DeleteAsync(location, default);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        content.Should().BeEmpty();
    }
}
