namespace Departments.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.SharedKernel;

using Domain;

public interface IDepartmentsRwRepository : IRwRepository<Department>
{
    Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default);
}
