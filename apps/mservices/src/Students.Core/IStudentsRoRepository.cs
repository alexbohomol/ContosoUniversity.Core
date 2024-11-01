namespace Students.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using Paging;

using Projections;

public interface IStudentsRoRepository
{
    Task<Student[]> GetStudentsEnrolledForCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);

    Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken = default);

    Task<PagedResult<Student>> Search(
        SearchRequest searchRequest,
        OrderRequest orderRequest,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default);

    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<Student> GetById(Guid entityId, CancellationToken cancellationToken = default);
}
