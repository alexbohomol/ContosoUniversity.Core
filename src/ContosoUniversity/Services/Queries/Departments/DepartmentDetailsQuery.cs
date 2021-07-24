namespace ContosoUniversity.Services.Queries.Departments
{
    using System;

    using MediatR;

    using ViewModels.Departments;

    public record DepartmentDetailsQuery(Guid Id) : IRequest<DepartmentDetailsViewModel>;
}