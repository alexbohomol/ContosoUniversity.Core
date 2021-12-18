namespace ContosoUniversity.Data.Departments.Writes;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts;

using Domain.Department;

using Microsoft.EntityFrameworkCore;

internal class DepartmentsReadWriteRepository : EfRwRepository<Department>, IDepartmentsRwRepository
{
    public DepartmentsReadWriteRepository(ReadWriteContext dbContext) : base(dbContext)
    {
    }

    public Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default)
    {
        return DbSet
            .Where(x => x.AdministratorId != null && x.AdministratorId == instructorId)
            .ToArrayAsync(cancellationToken);
    }
}