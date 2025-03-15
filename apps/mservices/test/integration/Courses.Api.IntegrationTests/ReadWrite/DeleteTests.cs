namespace Courses.Api.IntegrationTests.ReadWrite;

using System.Net;

using ContosoUniversity.Messaging.Contracts.Notifications;

using FluentAssertions;

using Models;

public class DeleteTests :
    IClassFixture<TestsConfiguration>,
    IClassFixture<DefaultApplicationFactory>,
    IClassFixture<InfrastructureContext>,
    IClassFixture<RabbitMqContext>
{
    private readonly HttpClient _httpClient;
    private readonly RabbitMqClient _rabbitMqClient;

    public DeleteTests(
        TestsConfiguration config,
        DefaultApplicationFactory factory,
        InfrastructureContext msSqlContext,
        RabbitMqContext rabbitMqContext)
    {
        factory.RabbitMqConnectionSetterFunction = () => rabbitMqContext.ConnectionString;
        _rabbitMqClient = new RabbitMqClient(rabbitMqContext.ConnectionString);
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CourseExists_ReturnsNoContent()
    {
        // Arrange
        (CreateCourseResponse created, Uri location) = await _httpClient.CreateCourse(Requests.Create.Course.Valid);

        // Act
        var response = await _httpClient.DeleteAsync(location, default);
        var content = await response.Content.ReadAsStringAsync();

        // Assert api response

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        content.Should().BeEmpty();

        // Assert message bus events

        var dptEvent = await _rabbitMqClient.TryConsumeAsync<CourseDeletedEvent>("course-deleted-event-handler-departments");
        dptEvent.Should().NotBeNull();
        dptEvent.Id.Should().Be(created.ExternalId);

        var stdEvent = await _rabbitMqClient.TryConsumeAsync<CourseDeletedEvent>("course-deleted-event-handler-students");
        stdEvent.Should().NotBeNull();
        stdEvent.Id.Should().Be(created.ExternalId);
    }
}
