namespace Courses.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

public interface ICoursesRoRepository
{
    Task<Course[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> GetCourseTitlesReference(
        Guid[] entityIds,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default);

    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<Course> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<Course[]> GetAll(CancellationToken cancellationToken = default);
}
