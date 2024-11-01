using Department = Departments.Core.Projections.Department;
using IDepartmentsRoRepository = Departments.Core.IDepartmentsRoRepository;

namespace ContosoUniversity.Application.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

public record GetDepartmentsIndexQuery : IRequest<Department[]>;

internal class GetDepartmentsIndexQueryHandler(IDepartmentsRoRepository departmentsRepository)
    : IRequestHandler<GetDepartmentsIndexQuery, Department[]>
{
    public async Task<Department[]> Handle(
        GetDepartmentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        return await departmentsRepository.GetAll(cancellationToken);
    }
}
