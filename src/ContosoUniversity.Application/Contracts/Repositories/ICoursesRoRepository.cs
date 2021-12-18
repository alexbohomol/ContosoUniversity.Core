namespace ContosoUniversity.Application.Contracts.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

using ReadModels;

public interface ICoursesRoRepository : IRoRepository<CourseReadModel>
{
    Task<CourseReadModel[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default);
    Task<CourseReadModel[]> GetByIds(Guid[] entityIds, CancellationToken cancellationToken = default);
    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default);
}