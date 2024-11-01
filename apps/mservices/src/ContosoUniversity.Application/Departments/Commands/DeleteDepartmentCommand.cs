using ICoursesRoRepository = Courses.Core.ICoursesRoRepository;
using IDepartmentsRwRepository = Departments.Core.IDepartmentsRwRepository;

namespace ContosoUniversity.Application.Departments.Commands;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
