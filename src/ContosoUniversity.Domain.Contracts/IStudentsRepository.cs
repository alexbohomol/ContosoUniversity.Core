namespace ContosoUniversity.Domain.Contracts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Paging;

    using Student;

    public interface IStudentsRepository : IRepository<Student>
    {
        Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken = default);
        Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken = default);
        Task<PagedResult<Student>> Search(
            SearchRequest searchRequest, 
            OrderRequest orderRequest, 
            PageRequest pageRequest,
            CancellationToken cancellationToken = default);
    }
}