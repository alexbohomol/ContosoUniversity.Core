namespace ContosoUniversity.Services.Departments.Queries
{
    using System;

    using MediatR;

    using ViewModels.Departments;

    public record DepartmentEditFormQuery(Guid Id) : IRequest<DepartmentEditForm>;
}