namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts;

using MassTransit;

using MediatR;

internal class DeleteDepartmentCommandHandler(
    IDepartmentsRwRepository departmentsRepository,
    IPublishEndpoint bus)
    : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await departmentsRepository.Remove(request.Id, cancellationToken);

        /*
         * remove courses and their related assignments and enrollments
         */
        await bus.Publish(
            new DepartmentDeletedEvent(request.Id),
            cancellationToken);
    }
}
