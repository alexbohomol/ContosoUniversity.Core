namespace ContosoUniversity.Data.Departments;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

public class InstructorsRepository : EfRepository<Instructor>, IInstructorsRepository
{
    public InstructorsRepository(DepartmentsContext dbContext) : base(dbContext)
    {
    }

    public Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.FullName,
                cancellationToken);
    }
}