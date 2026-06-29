namespace ContosoUniversity.Application.Contracts.Repositories.Writes;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Domain.Instructor;

public interface IInstructorsRwRepository : IRwRepository<Instructor>
{
    Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);
}
