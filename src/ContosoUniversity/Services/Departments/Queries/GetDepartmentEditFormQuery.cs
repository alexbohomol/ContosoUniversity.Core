namespace ContosoUniversity.Services.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;

using MediatR;

using ViewModels;
using ViewModels.Departments;

public record GetDepartmentEditFormQuery(Guid Id) : IRequest<EditDepartmentForm>;

public class GetDepartmentEditFormQueryHandler : IRequestHandler<GetDepartmentEditFormQuery, EditDepartmentForm>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly IInstructorsRoRepository _instructorsRepository;

    public GetDepartmentEditFormQueryHandler(
        IInstructorsRoRepository instructorsRepository,
        IDepartmentsRoRepository departmentsRepository)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<EditDepartmentForm> Handle(GetDepartmentEditFormQuery request,
        CancellationToken cancellationToken)
    {
        Department department = await _departmentsRepository.GetById(request.Id, cancellationToken);

        Dictionary<Guid, string> instructorNames =
            await _instructorsRepository.GetInstructorNamesReference(cancellationToken);

        return department == null
            ? null
            : new EditDepartmentForm
            {
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                AdministratorId = department.AdministratorId,
                ExternalId = department.ExternalId,
                // RowVersion = department.RowVersion,
                InstructorsDropDown = instructorNames.ToSelectList(department.AdministratorId ?? default)
            };
    }
}