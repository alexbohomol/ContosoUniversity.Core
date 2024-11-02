namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record DeleteDepartmentCommand(Guid Id) : IRequest;
