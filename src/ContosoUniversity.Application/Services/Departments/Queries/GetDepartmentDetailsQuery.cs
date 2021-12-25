namespace ContosoUniversity.Application.Services.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetDepartmentDetailsQuery(Guid Id) : IRequest<Department>;

public class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, Department>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;

    public GetDepartmentDetailsQueryHandler(IDepartmentsRoRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;
    }

    public async Task<Department> Handle(
        GetDepartmentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Department department = await _departmentsRepository.GetById(request.Id, cancellationToken);

        return department ?? throw new EntityNotFoundException(nameof(department), request.Id);
    }
}