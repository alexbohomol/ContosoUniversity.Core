namespace ContosoUniversity.Application.Services.Departments.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using Domain.Department;

using Exceptions;

using MediatR;

public class EditDepartmentCommand : IRequest
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public Guid? AdministratorId { get; set; }
    public Guid ExternalId { get; set; }
    public byte[] RowVersion { get; set; }
}

internal class EditDepartmentCommandHandler(
    IInstructorsRoRepository instructorsRepository,
    IDepartmentsRwRepository departmentsRepository) : IRequestHandler<EditDepartmentCommand>
{
    public async Task Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
    {
        Department department = await departmentsRepository.GetById(request.ExternalId, cancellationToken);
        if (department is null)
        {
            throw new EntityNotFoundException(nameof(department), request.ExternalId);
        }

        department.UpdateGeneralInfo(request.Name, request.Budget, request.StartDate);

        if (request.AdministratorId.HasValue)
        {
            if (!await instructorsRepository.Exists(request.AdministratorId.Value, cancellationToken))
            {
                throw new EntityNotFoundException("instructor", request.AdministratorId.Value);
            }

            department.AssociateAdministrator(request.AdministratorId.Value);
        }
        else
        {
            department.DisassociateAdministrator();
        }

        await departmentsRepository.Save(department, cancellationToken);
    }
}
