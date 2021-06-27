namespace ContosoUniversity.Services.Commands.Students
{
    using System;

    using MediatR;

    public record DeleteStudentCommand(Guid Id) : IRequest;
}