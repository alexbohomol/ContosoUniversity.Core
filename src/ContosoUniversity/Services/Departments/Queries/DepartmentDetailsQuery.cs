namespace ContosoUniversity.Services.Departments.Queries
{
    using System;

    using MediatR;

    using ViewModels.Departments;

    public record DepartmentDetailsQuery(Guid Id) : IRequest<DepartmentDetailsViewModel>;
}