namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Instructor;

public interface IInstructorsRepository : IRwRepository<Instructor>, IRoRepository<Instructor>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}