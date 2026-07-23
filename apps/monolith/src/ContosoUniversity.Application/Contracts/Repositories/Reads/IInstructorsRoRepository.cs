namespace ContosoUniversity.Application.Contracts.Repositories.Reads;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Projections;

using SharedKernel;

public interface IInstructorsRoRepository : IRoRepository<Instructor>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}
