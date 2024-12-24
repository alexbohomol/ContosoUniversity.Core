namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class DeleteDepartmentCommandHandler(IDepartmentsRwRepository departmentsRepository)
    : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await departmentsRepository.Remove(request.Id, cancellationToken);

        // /*
        //  * TODO: should be re-designed:
        //  * - we make dependent call after saving domain entity (domain transaction completes)
        //  * - this will cause inconsistency over boundaries when this call will fail
        //  * - notice 'courses/departments/:id' url here - indicator of wrong established boundaries
        //  */
        // Guid[] relatedCoursesIds = Enumerable
        //     .ToArray<Guid>((await coursesApiClient.GetByDepartmentId(request.Id, cancellationToken))
        //         .Select(x => x.ExternalId));
        //
        // /*
        //  * - remove related courses
        //  * - withdraw enrolled students
        //  * - remove related assignments (restrain assigned instructors)
        //  */
        // if (relatedCoursesIds.Any())
        // {
        //     await mediator.Publish(
        //         new DepartmentDeletedNotification(relatedCoursesIds),
        //         cancellationToken);
        // }
    }
}
