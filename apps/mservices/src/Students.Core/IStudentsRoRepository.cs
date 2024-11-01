namespace Students.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.SharedKernel;
using ContosoUniversity.SharedKernel.Paging;

using Projections;

public interface IStudentsRoRepository : IRoRepository<Student>
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
}
