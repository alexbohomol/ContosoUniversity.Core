namespace Departments.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

public interface IInstructorsRoRepository
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);

    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<Instructor> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<Instructor[]> GetAll(CancellationToken cancellationToken = default);
}
