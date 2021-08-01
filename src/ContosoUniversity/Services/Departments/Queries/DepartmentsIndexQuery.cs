namespace ContosoUniversity.Services.Departments.Queries
{
    using System.Collections.Generic;

    using MediatR;

    using ViewModels.Departments;

    public record DepartmentsIndexQuery : IRequest<IList<DepartmentListItemViewModel>>;
}