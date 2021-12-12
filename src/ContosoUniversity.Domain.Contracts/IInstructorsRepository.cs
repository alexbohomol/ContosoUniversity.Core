namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Instructor;

public interface IInstructorsRepository : IInstructorsRwRepository, IInstructorsRoRepository
{
    [Obsolete(
        "Temporarily hides original `GetById` methods from both bases. Needed to support both bases in one implementation")]
    new Task<Instructor> GetById(Guid entityId, CancellationToken cancellationToken = default);
}