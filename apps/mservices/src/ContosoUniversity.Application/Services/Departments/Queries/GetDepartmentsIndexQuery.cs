namespace ContosoUniversity.Application.Services.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Departments.Core;
using global::Departments.Core.Projections;

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
