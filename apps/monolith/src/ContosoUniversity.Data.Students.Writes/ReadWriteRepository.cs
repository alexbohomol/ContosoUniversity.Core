namespace ContosoUniversity.Data.Students.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

internal sealed class ReadWriteRepository(ReadWriteContext dbContext) : EfRwRepository<Student>(dbContext), IStudentsRwRepository
{
    public async Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        var ids = courseIds.ToList();

        return await DbQuery
            .Where(x => x.Enrollments.Any(e => ids.Contains(e.CourseId)))
            .ToArrayAsync(cancellationToken);
    }
}
