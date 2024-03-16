namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

public interface IDepartmentsRoRepository : IRoRepository<Department>
{
    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
}
