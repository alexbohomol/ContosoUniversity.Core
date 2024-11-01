using ICoursesRwRepository = Courses.Core.ICoursesRwRepository;

namespace ContosoUniversity.Application.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Notifications;

public record DeleteCourseCommand(Guid Id) : IRequest;

internal class DeleteCourseCommandHandler(
    ICoursesRwRepository coursesRwRepository,
    IMediator mediator)
    : IRequestHandler<DeleteCourseCommand>
{
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await coursesRwRepository.Remove(request.Id, cancellationToken);

        /*
         * remove related assignments and enrollments
         */
        await mediator.Publish(
            new CourseDeletedNotification(request.Id),
            cancellationToken);
    }
}
