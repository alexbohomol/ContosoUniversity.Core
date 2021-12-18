namespace ContosoUniversity.Application.Contracts.ReadModels;

using System;

using Domain;

public record CourseReadModel(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;