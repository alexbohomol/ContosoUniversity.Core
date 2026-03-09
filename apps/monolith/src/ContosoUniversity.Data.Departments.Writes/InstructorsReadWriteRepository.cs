namespace ContosoUniversity.Data.Departments.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

internal class InstructorsReadWriteRepository(ReadWriteContext dbContext) : EfRwRepository<Instructor>(dbContext), IInstructorsRwRepository
{
    public async Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        var ids = courseIds.ToList();

        return await DbQuery
            .Where(x => x.CourseAssignments.Any(e => ids.Contains(e.CourseId)))
            .ToArrayAsync(cancellationToken);
    }
}
