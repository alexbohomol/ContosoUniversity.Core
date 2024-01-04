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

public class DeleteCourseCommandHandler : AsyncRequestHandler<DeleteCourseCommand>
{
    private readonly ICoursesRoRepository _coursesRoRepository;
    private readonly ICoursesRwRepository _coursesRwRepository;
    private readonly IMediator _mediator;

    public DeleteCourseCommandHandler(
        ICoursesRoRepository coursesRoRepository,
        ICoursesRwRepository coursesRwRepository,
        IMediator mediator)
    {
        _coursesRoRepository = coursesRoRepository;
        _coursesRwRepository = coursesRwRepository;
        _mediator = mediator;
    }

    protected override async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
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
