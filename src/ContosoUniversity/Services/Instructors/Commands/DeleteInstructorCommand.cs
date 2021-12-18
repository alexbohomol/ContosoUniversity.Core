namespace ContosoUniversity.Services.Instructors.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories;
using Application.Exceptions;

using Domain.Department;

using MediatR;

public record DeleteInstructorCommand(Guid Id) : IRequest;

public class DeleteInstructorCommandHandler : AsyncRequestHandler<DeleteInstructorCommand>
{
    private readonly IDepartmentsRwRepository _departmentsRepository;
    private readonly IInstructorsRoRepository _instructorsRoRepository;
    private readonly IInstructorsRwRepository _instructorsRwRepository;

    public DeleteInstructorCommandHandler(
        IInstructorsRwRepository instructorsRwRepository,
        IInstructorsRoRepository instructorsRoRepository,
        IDepartmentsRwRepository departmentsRepository)
    {
        _instructorsRwRepository = instructorsRwRepository;
        _instructorsRoRepository = instructorsRoRepository;
        _departmentsRepository = departmentsRepository;
    }

    protected override async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        if (!await _instructorsRoRepository.Exists(request.Id, cancellationToken))
            throw new EntityNotFoundException("instructor", request.Id);

        Department[] administratedDepartments = await _departmentsRepository.GetByAdministrator(
            request.Id,
            cancellationToken);
        foreach (Department department in administratedDepartments)
        {
            department.DisassociateAdministrator();
            await _departmentsRepository.Save(department, cancellationToken);
        }

        await _instructorsRwRepository.Remove(request.Id, cancellationToken);
    }
}