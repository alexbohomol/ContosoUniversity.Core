namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record CreateDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId) : IRequest;
