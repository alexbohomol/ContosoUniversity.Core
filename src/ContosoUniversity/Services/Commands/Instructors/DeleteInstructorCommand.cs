namespace ContosoUniversity.Services.Commands.Instructors
{
    using System;

    using MediatR;

    public record DeleteInstructorCommand(Guid Id) : IRequest;
}