namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Course;

public interface ICoursesRepository : IRwRepository<Course>, IRoRepository<Course>
{
    Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default);
    Task<Course[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default);
    Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default);
    Task<Course[]> GetByIds(Guid[] entityIds, CancellationToken cancellationToken = default);
    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default);
}