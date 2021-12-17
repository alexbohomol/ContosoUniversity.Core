namespace ContosoUniversity.Application;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Domain.Department;

public interface IDepartmentsRoRepository : IRoRepository<DepartmentReadModel>
{
    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
}