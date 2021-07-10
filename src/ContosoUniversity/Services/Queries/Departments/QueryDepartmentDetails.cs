namespace ContosoUniversity.Services.Queries.Departments
{
    using System;

    using MediatR;

    using ViewModels.Departments;

    public record QueryDepartmentDetails(Guid Id) : IRequest<DepartmentDetailsViewModel>;
}