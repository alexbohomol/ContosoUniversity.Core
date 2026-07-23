namespace ContosoUniversity.Application.Contracts.Repositories.Reads;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

using SharedKernel;

public interface IDepartmentsRoRepository : IRoRepository<Department>
{
    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
}
