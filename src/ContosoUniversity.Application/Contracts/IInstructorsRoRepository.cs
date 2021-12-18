namespace ContosoUniversity.Application.Contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Domain.Instructor;

public interface IInstructorsRoRepository : IRoRepository<InstructorReadModel>
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default);
}