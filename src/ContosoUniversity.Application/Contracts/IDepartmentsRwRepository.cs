namespace ContosoUniversity.Application.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Department;

public interface IDepartmentsRwRepository : IRwRepository<Department>
{
    Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default);
}