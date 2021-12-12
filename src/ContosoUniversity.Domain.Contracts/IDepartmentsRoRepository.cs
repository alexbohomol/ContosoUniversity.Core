namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Department;

public interface IDepartmentsRoRepository : IRoRepository<Department>
{
    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
}