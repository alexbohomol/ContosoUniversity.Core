namespace ContosoUniversity.Services.Departments.Commands;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories;
using Application.Exceptions;

using MediatR;

using Notifications;

public record DeleteDepartmentCommand(Guid Id) : IRequest;

public class DeleteDepartmentCommandHandler : AsyncRequestHandler<DeleteDepartmentCommand>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRoRepository;
    private readonly IDepartmentsRwRepository _departmentsRwRepository;
    private readonly IMediator _mediator;

    public DeleteDepartmentCommandHandler(
        IDepartmentsRwRepository departmentsRwRepository,
        IDepartmentsRoRepository departmentsRoRepository,
        ICoursesRoRepository coursesRepository,
        IMediator mediator)
    {
        _departmentsRwRepository = departmentsRwRepository;
        _departmentsRoRepository = departmentsRoRepository;
        _coursesRepository = coursesRepository;
        _mediator = mediator;
    }

    protected override async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (!await _departmentsRoRepository.Exists(request.Id, cancellationToken))
            throw new EntityNotFoundException("department", request.Id);

        await _departmentsRwRepository.Remove(request.Id, cancellationToken);

        Guid[] relatedCoursesIds = (await _coursesRepository.GetByDepartmentId(request.Id, cancellationToken))
            .Select(x => x.ExternalId)
            .ToArray();

        /*
         * - remove related courses
         * - withdraw enrolled students
         * - remove related assignments (restrain assigned instructors)
         */
        if (relatedCoursesIds.Any())
            await _mediator.Publish(
                new DepartmentDeletedNotification(relatedCoursesIds),
                cancellationToken);
    }
}