namespace ContosoUniversity.Application.Contracts.Repositories.Writes;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Instructor;

using SharedKernel;

public interface IInstructorsRwRepository : IRwRepository<Instructor>
{
    Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);
}
