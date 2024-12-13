namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record DeleteInstructorCommand(Guid Id) : IRequest;
