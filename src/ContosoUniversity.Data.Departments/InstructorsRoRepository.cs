namespace ContosoUniversity.Data.Departments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

public class InstructorsRoRepository : EfRoRepository<Instructor>, IInstructorsRoRepository
{
    public InstructorsRoRepository(DepartmentsContext dbContext)
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