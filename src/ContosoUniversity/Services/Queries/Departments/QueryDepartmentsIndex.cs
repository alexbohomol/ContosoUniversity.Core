namespace ContosoUniversity.Services.Queries.Departments
{
    using System.Collections.Generic;

    using MediatR;

    using ViewModels.Departments;

    public record QueryDepartmentsIndex : IRequest<IList<DepartmentListItemViewModel>>;
}