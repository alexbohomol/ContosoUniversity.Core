namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record CreateCourseCommand(
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId) : IRequest;
