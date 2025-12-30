namespace ContosoUniversity.Application.ApiClients;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface ICoursesApiClient
{
    //Read-Only

    Task<Course> GetById(Guid externalId, CancellationToken cancellationToken);
    Task<Course[]> GetAll(CancellationToken cancellationToken);
    Task<bool> Exists(Guid externalId, CancellationToken cancellationToken);
    Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken);
    Task<Dictionary<Guid, string>> GetCourseTitlesReference(Guid[] entityIds, CancellationToken cancellationToken);

    //Read-Write

    Task Create(CourseCreateModel model, CancellationToken cancellationToken);
    Task Update(CourseEditModel model, CancellationToken cancellationToken);
    Task Delete(CourseDeleteModel model, CancellationToken cancellationToken);
    Task<int> UpdateCoursesCredits(int multiplier, CancellationToken cancellationToken);
}

public record Course(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId);

public record CourseCreateModel(
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId);

public record CourseDeleteModel(Guid Id);

public record CourseEditModel(
    Guid Id,
    string Title,
    int Credits,
    Guid DepartmentId);
