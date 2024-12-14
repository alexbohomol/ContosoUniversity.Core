namespace Students.Core.Handlers.Commands;

using System;

using MediatR;

public record DeleteStudentCommand(Guid Id) : IRequest;
