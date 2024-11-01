namespace Students.Data.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Data;

using Core;
using Core.Domain;

using Microsoft.EntityFrameworkCore;

internal sealed class ReadWriteRepository(ReadWriteContext dbContext) : EfRwRepository<Student>(dbContext), IStudentsRwRepository
{
    public async Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .Where(x => x.Enrollments.Select(e => e.CourseId).Any(id => courseIds.Contains(id)))
            .ToArrayAsync(cancellationToken);
    }
}
