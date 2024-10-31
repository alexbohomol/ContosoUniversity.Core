namespace ContosoUniversity.Data.Departments.Reads;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using global::Departments.Core;
using global::Departments.Core.Projections;

using Microsoft.EntityFrameworkCore;

internal class InstructorsReadOnlyRepository(ReadOnlyContext dbContext) : EfRoRepository<Instructor>(dbContext), IInstructorsRoRepository
{
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
