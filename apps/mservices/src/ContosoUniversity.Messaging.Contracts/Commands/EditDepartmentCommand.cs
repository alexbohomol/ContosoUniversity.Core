namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record EditDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId,
    byte[] RowVersion) : IRequest;
