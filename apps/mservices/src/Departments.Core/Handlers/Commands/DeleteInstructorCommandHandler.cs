using Department = Departments.Core.Domain.Department;

namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class DeleteInstructorCommandHandler(
    IInstructorsRwRepository instructorsRwRepository,
    IDepartmentsRwRepository departmentsRepository)
    : IRequestHandler<DeleteInstructorCommand>
{
    public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Department[] administratedDepartments = await departmentsRepository.GetByAdministrator(
            request.Id,
            cancellationToken);
        foreach (Department department in administratedDepartments)
        {
            department.DisassociateAdministrator();
            await departmentsRepository.Save(department, cancellationToken);
        }

        await instructorsRwRepository.Remove(request.Id, cancellationToken);
    }
}
