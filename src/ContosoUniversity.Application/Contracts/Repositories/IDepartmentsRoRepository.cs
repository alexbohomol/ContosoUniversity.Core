namespace ContosoUniversity.Application.Contracts.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ReadModels;

public interface IDepartmentsRoRepository : IRoRepository<DepartmentReadModel>
{
    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
}