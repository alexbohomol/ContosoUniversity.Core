namespace ContosoUniversity.Application.Contracts.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

using Paging;

using ReadModels;

public interface IStudentsRoRepository : IRoRepository<StudentReadModel>
{
    Task<StudentReadModel[]> GetStudentsEnrolledForCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);

    Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken = default);

    Task<PagedResult<StudentReadModel>> Search(
        SearchRequest searchRequest,
        OrderRequest orderRequest,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default);
}