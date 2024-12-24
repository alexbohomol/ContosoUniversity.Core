namespace ContosoUniversity.Application.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

using SharedKernel.Exceptions;

public record GetDepartmentEditFormQuery(Guid Id) : IRequest<GetDepartmentEditFormQueryResult>;

public record GetDepartmentEditFormQueryResult(
    Department Department,
    Dictionary<Guid, string> InstructorsReference);

internal class GetDepartmentEditFormQueryHandler(
    IInstructorsApiClient instructorsApiClient,
    IDepartmentsApiClient departmentsApiClient)
    : IRequestHandler<GetDepartmentEditFormQuery, GetDepartmentEditFormQueryResult>
{
    public async Task<GetDepartmentEditFormQueryResult> Handle(
        GetDepartmentEditFormQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Department department = await departmentsApiClient.GetById(request.Id, cancellationToken);
        if (department is null)
        {
            throw new EntityNotFoundException(nameof(department));
        }

        Dictionary<Guid, string> instructorNames = await instructorsApiClient.GetInstructorNamesReference(cancellationToken);

        return new GetDepartmentEditFormQueryResult(department, instructorNames);
    }
}
