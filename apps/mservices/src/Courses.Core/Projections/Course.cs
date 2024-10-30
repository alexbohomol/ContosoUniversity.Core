namespace Courses.Core.Projections;

using System;

using ContosoUniversity.Domain;

public record Course(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;
