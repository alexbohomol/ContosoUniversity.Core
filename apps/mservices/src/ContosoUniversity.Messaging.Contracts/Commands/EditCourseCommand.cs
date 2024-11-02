namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record EditCourseCommand(
    Guid Id,
    string Title,
    int Credits,
    Guid DepartmentId) : IRequest;
