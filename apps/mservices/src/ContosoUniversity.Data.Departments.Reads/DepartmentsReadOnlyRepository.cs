namespace ContosoUniversity.Data.Departments.Reads;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;

using Microsoft.EntityFrameworkCore;

internal class DepartmentsReadOnlyRepository(ReadOnlyContext dbContext) : EfRoRepository<Department>(dbContext), IDepartmentsRoRepository
{
    public Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .Select(x => new
            {
                x.ExternalId,
                x.Name
            })
            .OrderBy(x => x.Name)
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.Name,
                cancellationToken);
    }
}
