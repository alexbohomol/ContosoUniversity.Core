namespace ContosoUniversity.Domain.Course;

using System;

public record CourseReadModel(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;