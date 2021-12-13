namespace ContosoUniversity.Data.Departments.Reads;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Department;

using Microsoft.EntityFrameworkCore;

internal class DepartmentsRoRepository : EfRoRepository<Department>, IDepartmentsRoRepository
{
    public DepartmentsRoRepository(ReadOnlyContext dbContext) : base(dbContext)
    {
    }

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