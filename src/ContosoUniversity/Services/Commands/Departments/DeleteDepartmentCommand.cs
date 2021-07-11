namespace ContosoUniversity.Services.Commands.Departments
{
    using System;

    using MediatR;

    public record DeleteDepartmentCommand(Guid Id) : IRequest;
}