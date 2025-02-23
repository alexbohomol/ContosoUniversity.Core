namespace Courses.Core.Handlers.Commands;

using System;

using Domain;

using MediatR;

public record EditCourseCommand(
    Guid Id,
    string Title,
    int Credits,
    Guid DepartmentId) : IRequest<Course>;
