namespace ContosoUniversity.ApiClients;

using Application.ApiClients;

using SharedKernel.Paging;

internal class StudentsApiClient(HttpClient client) : IStudentsApiClient
{
    public Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<Student>> Search(SearchRequest searchRequest, OrderRequest orderRequest, PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Student> GetById(Guid externalId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
