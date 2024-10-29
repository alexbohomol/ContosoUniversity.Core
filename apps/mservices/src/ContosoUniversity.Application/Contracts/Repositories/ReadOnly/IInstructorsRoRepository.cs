namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

public interface IInstructorsRoRepository : IRoRepository<Instructor>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}
