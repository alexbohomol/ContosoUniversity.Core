namespace Departments.Core.Handlers.Commands;

using System;

using MediatR;

public record EditDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId,
    byte[] RowVersion) : IRequest;
