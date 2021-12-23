namespace ContosoUniversity.Application.Contracts.Repositories.ReadWrite;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Instructor;

public interface IInstructorsRwRepository : IRwRepository<Instructor>
{
    Task<Instructor[]> GetAllAssignedToCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken = default);
}