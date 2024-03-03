namespace ContosoUniversity.Application.Services.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetDepartmentEditFormQuery(Guid Id) : IRequest<GetDepartmentEditFormQueryResult>;

public record GetDepartmentEditFormQueryResult(
    Department Department,
    Dictionary<Guid, string> InstructorsReference);

internal class GetDepartmentEditFormQueryHandler :
    IRequestHandler<GetDepartmentEditFormQuery, GetDepartmentEditFormQueryResult>
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

    public async Task<GetDepartmentEditFormQueryResult> Handle(
        GetDepartmentEditFormQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Department department = await _departmentsRepository.GetById(request.Id, cancellationToken);
        if (department is null)
        {
            throw new EntityNotFoundException(nameof(department));
        }

        Dictionary<Guid, string> instructorNames = await _instructorsRepository
            .GetInstructorNamesReference(cancellationToken);

        return new GetDepartmentEditFormQueryResult(department, instructorNames);
    }
}
