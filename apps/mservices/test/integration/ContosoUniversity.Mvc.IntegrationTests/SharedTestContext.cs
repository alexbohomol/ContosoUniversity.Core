namespace ContosoUniversity.Mvc.IntegrationTests;

using System.Threading.Tasks;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

using Xunit;

public class SharedTestContext : IAsyncLifetime
{
    private WireMockServer _coursesApi;
    private WireMockServer _departmentsApi;
    private WireMockServer _studentsApi;

    public Task InitializeAsync()
    {
        _coursesApi = WireMockServer.Start(5006);
        _departmentsApi = WireMockServer.Start(5079);
        _studentsApi = WireMockServer.Start(5110);

        _coursesApi
            .Given(Request.Create()
                .WithPath("/api/courses")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody("[]")
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));

        _departmentsApi
            .Given(Request.Create()
                .WithPath("/api/departments/names")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody("{}")
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));

        _departmentsApi
            .Given(Request.Create()
                .WithPath("/api/departments")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody("[]")
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));

        _departmentsApi
            .Given(Request.Create()
                .WithPath("/api/instructors")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody("[]")
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));

        _studentsApi
            .Given(Request.Create()
                .WithPath("/api/students/enrolled/groups")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody("[]")
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _coursesApi.Stop();
        _coursesApi.Dispose();

        _departmentsApi.Stop();
        _departmentsApi.Dispose();

        _studentsApi.Stop();
        _studentsApi.Dispose();

        return Task.CompletedTask;
    }
}
