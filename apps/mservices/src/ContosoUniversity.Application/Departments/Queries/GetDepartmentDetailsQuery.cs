namespace ContosoUniversity.Application.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

using SharedKernel.Exceptions;

public record GetDepartmentDetailsQuery(Guid Id) : IRequest<Department>;

internal class GetDepartmentDetailsQueryHandler(IDepartmentsApiClient departmentsApiClient)
    : IRequestHandler<GetDepartmentDetailsQuery, Department>
{
    public async Task<Department> Handle(
        GetDepartmentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Department department = await departmentsApiClient.GetById(request.Id, cancellationToken);

        return department ?? throw new EntityNotFoundException(nameof(department), request.Id);
    }
}
