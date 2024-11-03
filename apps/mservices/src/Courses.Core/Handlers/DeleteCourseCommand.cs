namespace Courses.Core.Handlers;

using System;

using MediatR;

public record DeleteCourseCommand(Guid Id) : IRequest;
