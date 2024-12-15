namespace Departments.Core.Handlers.Commands;

using System;

using MediatR;

public record CreateDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId) : IRequest;
