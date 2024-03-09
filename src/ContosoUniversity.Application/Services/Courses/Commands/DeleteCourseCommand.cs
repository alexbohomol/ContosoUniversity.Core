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
    private readonly ICoursesRoRepository _coursesRoRepository = coursesRoRepository;
    private readonly ICoursesRwRepository _coursesRwRepository = coursesRwRepository;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        if (!await _coursesRoRepository.Exists(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("course", request.Id);
        }

        await _coursesRwRepository.Remove(request.Id, cancellationToken);

        /*
         * remove related assignments and enrollments
         */
        await _mediator.Publish(
            new CourseDeletedNotification(request.Id),
            cancellationToken);
    }
}
