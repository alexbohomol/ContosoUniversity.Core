namespace Departments.Api.IntegrationTests.ReadWrite.Departments;

using System.Net;

using FluentAssertions;

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
        var request = Requests.CreateDepartment.Valid;

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/departments", request, default);
        var createdDepartment = await response.Content.ReadFromJsonAsync<CreateDepartmentResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/api/departments/{createdDepartment.ExternalId}");
        createdDepartment.Should().BeEquivalentTo(request);
    }
}
