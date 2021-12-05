namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Instructor;

public interface IInstructorsRepository : IRwRepository<Instructor>, IRoRepository<Instructor>
{
    [Obsolete("Temporarily hides original `GetById` methods from both bases. Needed to support both bases in one implementation")]
    new Task<Instructor> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}