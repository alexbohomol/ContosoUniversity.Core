namespace Departments.Api.IntegrationTests.ReadWrite.Departments;

using System.Net;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

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
        (_, Uri location) = await _httpClient.CreateDepartment(Requests.Create.Department.Valid);
        var request = Requests.Update.Department.Valid;

        // Act
        var response = await _httpClient.PutAsJsonAsync(location, request, default);
        var updatedDepartment = await response.Content.ReadFromJsonAsync<UpdateDepartmentResponse>();

        // Assert
        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedDepartment.Should().BeEquivalentTo(request);
        location.OriginalString.Should().Contain(updatedDepartment.ExternalId.ToString());
    }
}
