namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record DeleteCourseCommand(Guid Id) : IRequest;
