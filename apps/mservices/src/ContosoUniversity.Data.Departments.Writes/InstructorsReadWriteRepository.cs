namespace ContosoUniversity.Data.Departments.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using global::Departments.Core;
using global::Departments.Core.Domain;

using Microsoft.EntityFrameworkCore;

internal class InstructorsReadWriteRepository(ReadWriteContext dbContext) : EfRwRepository<Instructor>(dbContext), IInstructorsRwRepository
{
    public async Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .Where(x => x.CourseAssignments.Select(e => e.CourseId).Any(id => courseIds.Contains(id)))
            .ToArrayAsync(cancellationToken);
    }
}
