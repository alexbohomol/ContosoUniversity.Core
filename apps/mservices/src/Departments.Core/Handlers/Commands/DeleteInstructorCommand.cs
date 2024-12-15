namespace Departments.Core.Handlers.Commands;

using System;

using MediatR;

public record DeleteInstructorCommand(Guid Id) : IRequest;
