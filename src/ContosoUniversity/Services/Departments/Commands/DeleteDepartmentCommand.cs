namespace ContosoUniversity.Services.Departments.Commands
{
    using System;

    using MediatR;

    public record DeleteDepartmentCommand(Guid Id) : IRequest;
}