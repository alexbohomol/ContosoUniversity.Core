namespace ContosoUniversity.Application.Contracts.Repositories.Writes;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Domain.Department;

public interface IDepartmentsRwRepository : IRwRepository<Department>
{
    Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default);
}
