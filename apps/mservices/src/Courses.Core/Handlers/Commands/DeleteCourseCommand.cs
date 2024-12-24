namespace Courses.Core.Handlers.Commands;

using System;

using MediatR;

public record DeleteCourseCommand(Guid Id) : IRequest;
