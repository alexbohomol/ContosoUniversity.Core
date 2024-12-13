namespace Courses.Core.Projections;

using System;

using ContosoUniversity.SharedKernel;

internal record Course(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;
