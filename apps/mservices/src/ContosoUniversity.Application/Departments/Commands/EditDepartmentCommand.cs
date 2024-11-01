using Department = Departments.Core.Domain.Department;
using IDepartmentsRwRepository = Departments.Core.IDepartmentsRwRepository;

namespace ContosoUniversity.Application.Departments.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

public record EditDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId,
    byte[] RowVersion) : IRequest;

internal class EditDepartmentCommandHandler(
    IDepartmentsRwRepository repository)
    : IRequestHandler<EditDepartmentCommand>
{
    public async Task Handle(
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
    }
}
