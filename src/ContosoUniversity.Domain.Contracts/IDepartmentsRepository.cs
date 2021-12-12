namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Department;

public interface IDepartmentsRepository : IDepartmentsRoRepository, IDepartmentsRwRepository
{
    [Obsolete(
        "Temporarily hides original `GetById` methods from both bases. Needed to support both bases in one implementation")]
    new Task<Department> GetById(Guid entityId, CancellationToken cancellationToken = default);
}