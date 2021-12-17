namespace ContosoUniversity.Services.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application;
using Application.Exceptions;

using Domain.Department;
using Domain.Instructor;

using MediatR;

using ViewModels.Departments;

public record GetDepartmentDetailsQuery(Guid Id) : IRequest<DepartmentDetailsViewModel>;

public class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, DepartmentDetailsViewModel>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly IInstructorsRoRepository _instructorsRepository;

    public GetDepartmentDetailsQueryHandler(
        IInstructorsRoRepository instructorsRepository,
        IDepartmentsRoRepository departmentsRepository)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<DepartmentDetailsViewModel> Handle(GetDepartmentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        DepartmentReadModel department = await _departmentsRepository.GetById(request.Id, cancellationToken);
        if (department is null)
            throw new EntityNotFoundException(nameof(department), request.Id);

        string fullname = string.Empty;
        if (department.AdministratorId.HasValue)
        {
            InstructorReadModel administrator = await _instructorsRepository.GetById(
                department.AdministratorId.Value,
                cancellationToken);

            if (administrator is null)
                throw new EntityNotFoundException(nameof(administrator), department.AdministratorId.Value);
            fullname = administrator.FullName;
        }

        return new DepartmentDetailsViewModel
        {
            Name = department.Name,
            Budget = department.Budget,
            StartDate = department.StartDate,
            Administrator = fullname,
            ExternalId = department.ExternalId
        };
    }
}