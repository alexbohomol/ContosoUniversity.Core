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
                // """
                // {
                //   "31a130fe-b396-4bb8-88d3-26fa8778b4c6": "Economics",
                //   "dab7e678-e3e7-4471-8282-96fe52e5c16f": "Engineering",
                //   "377c186a-6782-4367-9246-e5fe4195a97c": "English",
                //   "72c0804d-b208-4e67-82ba-cf54dc93dcc8": "Mathematics"
                // }
                // """)
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

        //         _studentsApi
        //             .Given(Request.Create()
        //                 .WithPath("/api/students/search")
        //                 .WithBodyAsJson(
        // """
        // {
        //     "SearchRequest": { "SearchString": null },
        //     "OrderRequest": { "SortOrder": null },
        //     "PageRequest": { "PageNumber": 1, "PageSize": 3 }
        // }
        // """)
        //                 .UsingPost())
        //             .RespondWith(Response.Create()
        //                 .WithBodyAsJson(
        // """
        // {
        //     "Items": [],
        //     "Info": { "PageIndex": 1, "HasPreviousPage": "false", "HasNextPage": "false" }
        // }
        // """)
        //                 .WithHeader("content-type", "application/json; charset=utf-8")
        //                 .WithStatusCode(200));

        //         _studentsApi
        //             .Given(Request.Create()
        //                 .WithPath("/api/students/search")
        //                 .WithBodyAsJson(new
        //                 {
        //                     SearchRequest = null as SearchRequest,
        //                     OrderRequest = null as OrderRequest,
        //                     PageRequest = new PageRequest(1, 3)
        //                 })
        //                 .UsingPost())
        //             .RespondWith(Response.Create()
        //                 .WithBodyAsJson(new
        // {
        //     Items = Array.Empty<>(),
        //     "Info": { "PageIndex": 1, "HasPreviousPage": "false", "HasNextPage": "false" }
        // })
        //                 .WithHeader("content-type", "application/json; charset=utf-8")
        //                 .WithStatusCode(200));
        //
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
