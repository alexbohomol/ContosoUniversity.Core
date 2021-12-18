namespace ContosoUniversity.Application.Contracts.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ReadModels;

public interface IInstructorsRoRepository : IRoRepository<InstructorReadModel>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}