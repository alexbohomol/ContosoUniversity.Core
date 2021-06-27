namespace ContosoUniversity.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Course;

    public interface ICoursesRepository : IRepository<Course>
    {
        Task<int> UpdateCourseCredits(int multiplier);
        Task<Course[]> GetByDepartmentId(Guid departmentId);
        Task Remove(Guid[] entityIds);
        Task<Course[]> GetByIds(Guid[] entityIds);
        Task<bool> ExistsCourseCode(int courseCode);
    }
}