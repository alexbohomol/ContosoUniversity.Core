namespace Departments.Api.IntegrationTests.ReadWrite.Departments;

using System.Net;

using ContosoUniversity.Messaging.Contracts;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using Models;

public class DeleteTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<MsSqlContext>,
    IClassFixture<RabbitMqContext>
{
    private readonly HttpClient _httpClient;
    private readonly RabbitMqClient _rabbitMqClient;

    public DeleteTests(
        TestsConfiguration config,
        DefaultApplicationFactory<IAssemblyMarker> factory,
        MsSqlContext msSqlContext,
        RabbitMqContext rabbitMqContext)
    {
        factory.RabbitMqConnectionSetterFunction = () => rabbitMqContext.ConnectionString;
        _rabbitMqClient = new RabbitMqClient(rabbitMqContext.ConnectionString);
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task DepartmentExists_ReturnsNoContent()
    {
        // Arrange
        (CreateDepartmentResponse created, Uri location) = await _httpClient.CreateDepartment(Requests.Create.Department.Valid);

        // Act
        var response = await _httpClient.DeleteAsync(location, default);
        var content = await response.Content.ReadAsStringAsync();

        // Assert api response

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        content.Should().BeEmpty();

        // Assert message bus event

        var dptEvent = await _rabbitMqClient.TryConsumeAsync<DepartmentDeletedEvent>("department-deleted-event-handler-courses");
        dptEvent.Should().NotBeNull();
        dptEvent.DepartmentId.Should().Be(created.ExternalId);
    }
}
