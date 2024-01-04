namespace ContosoUniversity.Data.Departments.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

internal class InstructorsReadWriteRepository : EfRwRepository<Instructor>, IInstructorsRwRepository
{
    public InstructorsReadWriteRepository(ReadWriteContext dbContext) : base(dbContext)
    {
    }

    public async Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .Where(x => x.CourseAssignments.Select(e => e.CourseId).Any(id => courseIds.Contains(id)))
            .ToArrayAsync(cancellationToken);
    }
}
