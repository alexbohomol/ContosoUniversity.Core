namespace ContosoUniversity.Services.Queries.Departments
{
    using System;

    using MediatR;

    using ViewModels.Departments;

    public record DepartmentEditFormQuery(Guid Id) : IRequest<DepartmentEditForm>;
}