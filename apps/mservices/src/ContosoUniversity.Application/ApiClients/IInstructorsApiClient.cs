namespace ContosoUniversity.Application.ApiClients;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IInstructorsApiClient
{
    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken);
    Task<Instructor[]> GetAll(CancellationToken cancellationToken);
    Task<Instructor> GetById(Guid externalId, CancellationToken cancellationToken);
}

public record Instructor(
    string FirstName,
    string LastName,
    DateTime HireDate,
    Guid[] Courses,
    string Office,
    Guid ExternalId);
