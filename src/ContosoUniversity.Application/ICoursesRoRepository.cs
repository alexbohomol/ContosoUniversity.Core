namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Course;

public interface ICoursesRoRepository : IRoRepository<CourseReadModel>
{
    Task<CourseReadModel[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default);
    Task<CourseReadModel[]> GetByIds(Guid[] entityIds, CancellationToken cancellationToken = default);
    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default);
}