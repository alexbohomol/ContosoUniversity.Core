namespace ContosoUniversity.Services.Students.Commands
{
    using System;

    using MediatR;

    public record DeleteStudentCommand(Guid Id) : IRequest;
}