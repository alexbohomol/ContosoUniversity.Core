using Department = Departments.Core.Projections.Department;
using IDepartmentsRoRepository = Departments.Core.IDepartmentsRoRepository;
using IInstructorsRoRepository = Departments.Core.IInstructorsRoRepository;

namespace ContosoUniversity.Application.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SharedKernel.Exceptions;

public record GetDepartmentEditFormQuery(Guid Id) : IRequest<GetDepartmentEditFormQueryResult>;

public record GetDepartmentEditFormQueryResult(
    Department Department,
    Dictionary<Guid, string> InstructorsReference);

internal class GetDepartmentEditFormQueryHandler(
    IInstructorsRoRepository instructorsRepository,
    IDepartmentsRoRepository departmentsRepository)
    : IRequestHandler<GetDepartmentEditFormQuery, GetDepartmentEditFormQueryResult>
{
    public async Task<GetDepartmentEditFormQueryResult> Handle(
        GetDepartmentEditFormQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Department department = await departmentsRepository.GetById(request.Id, cancellationToken);
        if (department is null)
        {
            throw new EntityNotFoundException(nameof(department));
        }

        Dictionary<Guid, string> instructorNames = await instructorsRepository
            .GetInstructorNamesReference(cancellationToken);

        return new GetDepartmentEditFormQueryResult(department, instructorNames);
    }
}
