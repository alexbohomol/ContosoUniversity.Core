namespace ContosoUniversity.Data.Students.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Student;

using Microsoft.EntityFrameworkCore;

internal sealed class ReadWriteRepository : EfRwRepository<Student>, IStudentsRwRepository
{
    public ReadWriteRepository(ReadWriteContext dbContext) : base(dbContext)
    {
    }

    public async Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .Where(x => x.Enrollments.Select(e => e.CourseId).Any(id => courseIds.Contains(id)))
            .ToArrayAsync(cancellationToken);
    }
}