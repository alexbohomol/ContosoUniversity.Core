namespace ContosoUniversity.Data.Departments.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using global::Departments.Core;
using global::Departments.Core.Domain;

using Microsoft.EntityFrameworkCore;

internal class DepartmentsReadWriteRepository(ReadWriteContext dbContext) : EfRwRepository<Department>(dbContext), IDepartmentsRwRepository
{
    public Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default)
    {
        return DbSet
            .Where(x => x.AdministratorId != null && x.AdministratorId == instructorId)
            .ToArrayAsync(cancellationToken);
    }
}
