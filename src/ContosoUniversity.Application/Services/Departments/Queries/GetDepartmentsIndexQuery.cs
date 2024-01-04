namespace ContosoUniversity.Application.Services.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using MediatR;

public record GetDepartmentsIndexQuery : IRequest<Department[]>;

public class GetDepartmentsIndexQueryHandler : IRequestHandler<GetDepartmentsIndexQuery, Department[]>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;

    public GetDepartmentsIndexQueryHandler(IDepartmentsRoRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;
    }

    public async Task<Department[]> Handle(
        GetDepartmentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        return await _departmentsRepository.GetAll(cancellationToken);
    }
}
