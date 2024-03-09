namespace ContosoUniversity.Application.Services.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetDepartmentDetailsQuery(Guid Id) : IRequest<Department>;

internal class GetDepartmentDetailsQueryHandler(IDepartmentsRoRepository departmentsRepository) : IRequestHandler<GetDepartmentDetailsQuery, Department>
{
    public async Task<Department> Handle(
        GetDepartmentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Department department = await departmentsRepository.GetById(request.Id, cancellationToken);

        return department ?? throw new EntityNotFoundException(nameof(department), request.Id);
    }
}
