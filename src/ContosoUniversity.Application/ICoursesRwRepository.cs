namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Course;

public interface ICoursesRwRepository : IRwRepository<Course>
{
    Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default);
    Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default);
}