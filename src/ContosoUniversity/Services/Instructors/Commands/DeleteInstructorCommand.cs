namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;

    using MediatR;

    public record DeleteInstructorCommand(Guid Id) : IRequest;
}