namespace ContosoUniversity.Application.ApiClients;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IInstructorsApiClient
{
    // Read-Only

    Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken);
    Task<Instructor[]> GetAll(CancellationToken cancellationToken);
    Task<Instructor> GetById(Guid externalId, CancellationToken cancellationToken);

    // Read-Write

    Task Create(InstructorCreateModel model, CancellationToken cancellationToken);
    Task Update(InstructorEditModel model, CancellationToken cancellationToken);
    Task Delete(InstructorDeleteModel model, CancellationToken cancellationToken);
}

public record Instructor(
    string FirstName,
    string LastName,
    DateTime HireDate,
    Guid[] Courses,
    string Office,
    Guid ExternalId);

public record InstructorCreateModel(
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);

public record InstructorEditModel(
    Guid ExternalId,
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);


public record InstructorDeleteModel(Guid Id);
