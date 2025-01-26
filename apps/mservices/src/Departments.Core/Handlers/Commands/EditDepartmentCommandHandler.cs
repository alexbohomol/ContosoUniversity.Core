namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class EditDepartmentCommandHandler(
    IDepartmentsRwRepository repository)
    : IRequestHandler<EditDepartmentCommand, Department>
{
    public async Task<Department> Handle(
        EditDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Department department = await repository.GetById(request.ExternalId, cancellationToken);

        ArgumentNullException.ThrowIfNull(department);

        department.UpdateGeneralInfo(request.Name, request.Budget, request.StartDate);

        if (request.AdministratorId.HasValue)
        {
            department.AssociateAdministrator(request.AdministratorId.Value);
        }
        else
        {
            department.DisassociateAdministrator();
        }

        await repository.Save(department, cancellationToken);

        return department;
    }
}
