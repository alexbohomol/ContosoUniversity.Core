namespace ContosoUniversity.Application.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using Exceptions;

using MediatR;

using Notifications;

public record DeleteCourseCommand(Guid Id) : IRequest;

internal class DeleteCourseCommandHandler(
    ICoursesRoRepository coursesRoRepository,
    ICoursesRwRepository coursesRwRepository,
    IMediator mediator) : IRequestHandler<DeleteCourseCommand>
{
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        if (!await coursesRoRepository.Exists(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("course", request.Id);
        }

        await coursesRwRepository.Remove(request.Id, cancellationToken);

        /*
         * remove related assignments and enrollments
         */
        await mediator.Publish(
            new CourseDeletedNotification(request.Id),
            cancellationToken);
    }
}
