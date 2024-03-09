namespace ContosoUniversity.Application.Services.Departments.Commands;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using Exceptions;

using MediatR;

using Notifications;

public record DeleteDepartmentCommand(Guid Id) : IRequest;

internal class DeleteDepartmentCommandHandler(
    IDepartmentsRwRepository departmentsRwRepository,
    IDepartmentsRoRepository departmentsRoRepository,
    ICoursesRoRepository coursesRepository,
    IMediator mediator) : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly ICoursesRoRepository _coursesRepository = coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRoRepository = departmentsRoRepository;
    private readonly IDepartmentsRwRepository _departmentsRwRepository = departmentsRwRepository;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (!await _departmentsRoRepository.Exists(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("department", request.Id);
        }

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
        {
            await _mediator.Publish(
                new DepartmentDeletedNotification(relatedCoursesIds),
                cancellationToken);
        }
    }
}
