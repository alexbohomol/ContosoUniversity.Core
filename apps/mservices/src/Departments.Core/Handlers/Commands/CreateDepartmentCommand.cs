namespace Departments.Core.Handlers.Commands;

using System;

using Domain;

using MediatR;

public record CreateDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId) : IRequest<Department>;
