namespace ContosoUniversity.Application.Services.Departments.Commands;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using MediatR;

using Notifications;

public record DeleteDepartmentCommand(Guid Id) : IRequest;

internal class DeleteDepartmentCommandHandler(
    IDepartmentsRwRepository departmentsRepository,
    ICoursesRoRepository coursesRepository,
    IMediator mediator)
    : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await departmentsRepository.Remove(request.Id, cancellationToken);

        Guid[] relatedCoursesIds = (await coursesRepository.GetByDepartmentId(request.Id, cancellationToken))
            .Select(x => x.ExternalId)
            .ToArray();

        /*
         * - remove related courses
         * - withdraw enrolled students
         * - remove related assignments (restrain assigned instructors)
         */
        if (relatedCoursesIds.Any())
        {
            await mediator.Publish(
                new DepartmentDeletedNotification(relatedCoursesIds),
                cancellationToken);
        }
    }
}
