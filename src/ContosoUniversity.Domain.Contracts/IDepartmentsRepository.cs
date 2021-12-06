namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Department;

public interface IDepartmentsRepository : IRwRepository<Department>, IRoRepository<Department>
{
    [Obsolete(
        "Temporarily hides original `GetById` methods from both bases. Needed to support both bases in one implementation")]
    new Task<Department> GetById(Guid entityId, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
    Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default);
}