namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Instructor;

public interface IInstructorsRepository : IRepository<Instructor>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}