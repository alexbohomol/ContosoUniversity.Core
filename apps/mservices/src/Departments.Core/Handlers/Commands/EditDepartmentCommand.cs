namespace Departments.Core.Handlers.Commands;

using System;

using Domain;

using MediatR;

public record EditDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId) : IRequest<Department>;
