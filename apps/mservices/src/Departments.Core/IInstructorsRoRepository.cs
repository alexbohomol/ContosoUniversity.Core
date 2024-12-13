namespace Departments.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.SharedKernel;

using Projections;

public interface IInstructorsRoRepository : IRoRepository<Instructor>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}
