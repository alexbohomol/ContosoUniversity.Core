namespace ContosoUniversity.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Paging;

    using Student;

    public interface IStudentsRepository : IRepository<Student>
    {
        Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups();
        Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds);
        Task<PagedResult<Student>> Search(
            SearchRequest searchRequest, 
            OrderRequest orderRequest, 
            PageRequest pageRequest);
    }
}