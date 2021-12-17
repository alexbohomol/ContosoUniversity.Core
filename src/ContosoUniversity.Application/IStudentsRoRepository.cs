namespace ContosoUniversity.Application;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Student;

using Paging;

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