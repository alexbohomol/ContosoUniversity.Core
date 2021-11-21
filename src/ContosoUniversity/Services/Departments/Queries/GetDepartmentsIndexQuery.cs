namespace ContosoUniversity.Services.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Department;

using MediatR;

using ViewModels.Departments;

public record GetDepartmentsIndexQuery : IRequest<IList<DepartmentListItemViewModel>>;

public class
    GetDepartmentsIndexQueryHandler : IRequestHandler<GetDepartmentsIndexQuery, IList<DepartmentListItemViewModel>>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IInstructorsRepository _instructorsRepository;

    public GetDepartmentsIndexQueryHandler(
        IInstructorsRepository instructorsRepository,
        IDepartmentsRepository departmentsRepository)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<IList<DepartmentListItemViewModel>> Handle(GetDepartmentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        Department[] departments = await _departmentsRepository.GetAll(cancellationToken);

        Dictionary<Guid, string> instructorsNames =
            await _instructorsRepository.GetInstructorNamesReference(cancellationToken);

        return departments.Select(x => new DepartmentListItemViewModel
        {
            Name = x.Name,
            Budget = x.Budget,
            StartDate = x.StartDate,
            Administrator = x.AdministratorId.HasValue
                ? instructorsNames.ContainsKey(x.AdministratorId.Value)
                    ? instructorsNames[x.AdministratorId.Value]
                    : string.Empty
                : string.Empty,
            ExternalId = x.ExternalId
        }).ToList();
    }
}