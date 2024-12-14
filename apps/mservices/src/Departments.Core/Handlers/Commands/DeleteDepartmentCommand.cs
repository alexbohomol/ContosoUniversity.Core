namespace Departments.Core.Handlers.Commands;

using System;

using MediatR;

public record DeleteDepartmentCommand(Guid Id) : IRequest;
