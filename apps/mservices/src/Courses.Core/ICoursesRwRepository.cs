namespace Courses.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.SharedKernel;

using Domain;

public interface ICoursesRwRepository : IRwRepository<Course>
{
    Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default);
    Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default);
}
