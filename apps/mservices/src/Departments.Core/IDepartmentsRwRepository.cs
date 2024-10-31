namespace Departments.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface IDepartmentsRwRepository
{
    Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default);

    Task<Department> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task Save(Department entity, CancellationToken cancellationToken = default);
    Task Remove(Guid entityId, CancellationToken cancellationToken = default);
}
