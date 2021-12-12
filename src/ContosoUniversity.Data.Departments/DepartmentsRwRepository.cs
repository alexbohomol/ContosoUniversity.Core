namespace ContosoUniversity.Data.Departments;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Department;

using Microsoft.EntityFrameworkCore;

public class DepartmentsRwRepository : EfRwRepository<Department>, IDepartmentsRwRepository
{
    public DepartmentsRwRepository(DepartmentsContext dbContext) : base(dbContext)
    {
    }

    public Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default)
    {
        return DbSet
            .Where(x => x.AdministratorId != null && x.AdministratorId == instructorId)
            .ToArrayAsync(cancellationToken);
    }
}