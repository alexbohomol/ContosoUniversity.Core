namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class CreateDepartmentCommandHandler(
    IDepartmentsRwRepository departmentsRepository)
    : IRequestHandler<CreateDepartmentCommand, Department>
{
    public async Task<Department> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var department = Department.Create(
            request.Name,
            request.Budget,
            request.StartDate,
            request.AdministratorId);

        await departmentsRepository.Save(department, cancellationToken);

        return department;
    }
}
