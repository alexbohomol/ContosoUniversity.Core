namespace Departments.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

public interface IDepartmentsRoRepository
{
    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);

    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<Department> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<Department[]> GetAll(CancellationToken cancellationToken = default);
}
