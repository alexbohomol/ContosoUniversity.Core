namespace ContosoUniversity.Services.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Exceptions;

using MediatR;

public record GetDepartmentDetailsQuery(Guid Id) : IRequest<GetDepartmentDetailsQueryResult>;

public record GetDepartmentDetailsQueryResult(Department Department, string InstructorName);

public class GetDepartmentDetailsQueryHandler :
    IRequestHandler<GetDepartmentDetailsQuery, GetDepartmentDetailsQueryResult>
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

    public async Task<GetDepartmentDetailsQueryResult> Handle(
        GetDepartmentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

#warning This is potential place to introduce view in scope of dpt-schema. Instructors names can be included in read model,
        Department department = await _departmentsRepository.GetById(request.Id, cancellationToken);
        if (department is null)
            throw new EntityNotFoundException(nameof(department), request.Id);

        string fullname = string.Empty;
        if (department.AdministratorId.HasValue)
        {
            Dictionary<Guid, string> instructorsReference = await _instructorsRepository
                .GetInstructorNamesReference(cancellationToken);

            if (!instructorsReference.ContainsKey(department.AdministratorId.Value))
                throw new EntityNotFoundException("administrator", department.AdministratorId.Value);

            fullname = instructorsReference[department.AdministratorId.Value];
        }

        return new GetDepartmentDetailsQueryResult(department, fullname);
    }
}