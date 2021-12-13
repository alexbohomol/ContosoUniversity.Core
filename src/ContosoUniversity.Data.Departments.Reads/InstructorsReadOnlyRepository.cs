namespace ContosoUniversity.Data.Departments.Reads;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

internal class InstructorsReadOnlyRepository : EfRoRepository<Instructor>, IInstructorsRoRepository
{
    public InstructorsReadOnlyRepository(ReadOnlyContext dbContext)
        : base(
            dbContext,
            new[]
            {
                nameof(Instructor.Assignments),
                nameof(Instructor.Office)
            })
    {
    }

    public Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .Select(x => new
            {
                x.ExternalId,
                FullName = $"{x.LastName}, {x.FirstName}"
            })
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.FullName,
                cancellationToken);
    }
}