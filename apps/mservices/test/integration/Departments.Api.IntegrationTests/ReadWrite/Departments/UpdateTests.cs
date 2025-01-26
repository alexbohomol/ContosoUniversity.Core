namespace Departments.Api.IntegrationTests.ReadWrite.Departments;

using System.Net;

using FluentAssertions;

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
        (_, Uri location) = await _httpClient.CreateDepartment(Requests.CreateDepartment.Valid);
        var request = Requests.UpdateDepartment.Valid;

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, request, default);
        var updatedDepartment = await response.Content.ReadFromJsonAsync<UpdateDepartmentResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // updatedDepartment.Should().BeEquivalentTo(request);
        // updatedDepartment.ExternalId.Should().Be(request.ExternalId);
        updatedDepartment.Name.Should().Be(request.Name);
        updatedDepartment.Budget.Should().Be(request.Budget);
        updatedDepartment.StartDate.Should().Be(request.StartDate);
        updatedDepartment.AdministratorId.Should().Be(request.AdministratorId);
    }
}
