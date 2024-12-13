namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record DeleteStudentCommand(Guid Id) : IRequest;
