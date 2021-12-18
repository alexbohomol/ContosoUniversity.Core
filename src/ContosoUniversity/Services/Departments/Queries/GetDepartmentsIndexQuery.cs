namespace ContosoUniversity.Services.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;

using MediatR;

using ViewModels.Departments;

public record GetDepartmentsIndexQuery : IRequest<IList<DepartmentListItemViewModel>>;

public class GetDepartmentsIndexQueryHandler
    : IRequestHandler<GetDepartmentsIndexQuery, IList<DepartmentListItemViewModel>>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly IInstructorsRoRepository _instructorsRepository;

    public GetDepartmentsIndexQueryHandler(
        IInstructorsRoRepository instructorsRepository,
        IDepartmentsRoRepository departmentsRepository)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<IList<DepartmentListItemViewModel>> Handle(GetDepartmentsIndexQuery request,
        CancellationToken cancellationToken)
    {
#warning This is potential place to introduce view in scope of dpt-schema. Instructors names can be included in read model,
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