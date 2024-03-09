namespace ContosoUniversity.Application.Services.Instructors.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using Domain.Department;

using Exceptions;

using MediatR;

public record DeleteInstructorCommand(Guid Id) : IRequest;

internal class DeleteInstructorCommandHandler(
    IInstructorsRwRepository instructorsRwRepository,
    IInstructorsRoRepository instructorsRoRepository,
    IDepartmentsRwRepository departmentsRepository) : IRequestHandler<DeleteInstructorCommand>
{
    public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        if (!await instructorsRoRepository.Exists(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("instructor", request.Id);
        }

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
