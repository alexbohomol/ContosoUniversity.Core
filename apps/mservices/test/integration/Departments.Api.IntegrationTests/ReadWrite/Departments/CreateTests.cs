namespace Departments.Api.IntegrationTests.ReadWrite.Departments;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

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
        var request = Requests.Create.Department.Valid;

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
