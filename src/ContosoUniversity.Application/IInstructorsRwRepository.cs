namespace ContosoUniversity.Application;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Instructor;

public interface IInstructorsRwRepository : IRwRepository<Instructor>
{
    [Obsolete("This is a quick fix. We need to introduce specific method for each query.")]
    Task<Instructor[]> GetAll(CancellationToken cancellationToken = default);
}