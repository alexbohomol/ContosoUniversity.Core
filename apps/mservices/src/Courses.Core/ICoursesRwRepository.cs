namespace Courses.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface ICoursesRwRepository
{
    Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default);
    Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default);

    Task<Course> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task Save(Course entity, CancellationToken cancellationToken = default);
    Task Remove(Guid entityId, CancellationToken cancellationToken = default);
}
