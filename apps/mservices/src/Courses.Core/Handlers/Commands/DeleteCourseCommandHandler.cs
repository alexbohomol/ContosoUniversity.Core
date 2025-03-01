namespace Courses.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MassTransit;

using MediatR;

internal class DeleteCourseCommandHandler(
    ICoursesRwRepository coursesRwRepository,
    IPublishEndpoint bus)
    : IRequestHandler<DeleteCourseCommand>
{
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await coursesRwRepository.Remove(request.Id, cancellationToken);

        /*
         * remove related assignments and enrollments
         */
        await bus.Publish(
            new CourseDeletedEvent(request.Id),
            cancellationToken);
    }
}
