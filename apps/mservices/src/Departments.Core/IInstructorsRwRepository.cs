namespace Departments.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.SharedKernel;

using Domain;

public interface IInstructorsRwRepository : IRwRepository<Instructor>
{
    Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);
}
