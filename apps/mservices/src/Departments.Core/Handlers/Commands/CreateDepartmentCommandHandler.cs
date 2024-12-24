using Department = Departments.Core.Domain.Department;

namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class CreateDepartmentCommandHandler(
    IDepartmentsRwRepository departmentsRepository)
    : IRequestHandler<CreateDepartmentCommand>
{
    public async Task Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await departmentsRepository.Save(
            Department.Create(
                request.Name,
                request.Budget,
                request.StartDate,
                request.AdministratorId),
            cancellationToken);
    }
}
