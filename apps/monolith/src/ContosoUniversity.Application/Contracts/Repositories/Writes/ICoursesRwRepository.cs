namespace ContosoUniversity.Application.Contracts.Repositories.Writes;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Domain.Course;

public interface ICoursesRwRepository : IRwRepository<Course>
{
    Task<int> UpdateCourseCredits(int multiplier, CancellationToken cancellationToken = default);
    Task Remove(Guid[] entityIds, CancellationToken cancellationToken = default);
}
