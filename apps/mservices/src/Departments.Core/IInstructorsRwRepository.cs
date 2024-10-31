namespace Departments.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface IInstructorsRwRepository
{
    Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);

    Task<Instructor> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task Save(Instructor entity, CancellationToken cancellationToken = default);
    Task Remove(Guid entityId, CancellationToken cancellationToken = default);
}
