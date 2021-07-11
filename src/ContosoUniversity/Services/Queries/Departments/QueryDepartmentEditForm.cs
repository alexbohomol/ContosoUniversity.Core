namespace ContosoUniversity.Services.Queries.Departments
{
    using System;

    using MediatR;

    using ViewModels.Departments;

    public record QueryDepartmentEditForm(Guid Id) : IRequest<DepartmentEditForm>;
}