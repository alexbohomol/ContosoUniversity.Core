namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Course;

public interface ICoursesRoRepository : IRoRepository<Course>
{
    Task<Course[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default);
    Task<Course[]> GetByIds(Guid[] entityIds, CancellationToken cancellationToken = default);
    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default);
}