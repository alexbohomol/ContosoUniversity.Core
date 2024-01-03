namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

public interface ICoursesRoRepository : IRoRepository<Course>
{
    Task<Course[]> GetByDepartmentId(Guid departmentId, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> GetCourseTitlesReference(
        Guid[] entityIds,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken = default);
}
