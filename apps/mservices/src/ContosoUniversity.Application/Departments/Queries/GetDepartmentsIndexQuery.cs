namespace ContosoUniversity.Application.Departments.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

public record GetDepartmentsIndexQuery : IRequest<Department[]>;

internal class GetDepartmentsIndexQueryHandler(IDepartmentsApiClient departmentsApiClient)
    : IRequestHandler<GetDepartmentsIndexQuery, Department[]>
{
    public async Task<Department[]> Handle(
        GetDepartmentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        return await departmentsApiClient.GetAll(cancellationToken);
    }
}
