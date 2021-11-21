namespace ContosoUniversity.Services.Courses.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Contracts.Exceptions;

using MediatR;

using Notifications;

public record DeleteCourseCommand(Guid Id) : IRequest;

public class DeleteCourseCommandHandler : AsyncRequestHandler<DeleteCourseCommand>
{
    private readonly ICoursesRepository _coursesRepository;
    private readonly IMediator _mediator;

    public DeleteCourseCommandHandler(
        ICoursesRepository coursesRepository,
        IMediator mediator)
    {
        _coursesRepository = coursesRepository;
        _mediator = mediator;
    }

    protected override async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        if (!await _coursesRepository.Exists(request.Id, cancellationToken))
            throw new EntityNotFoundException("course", request.Id);

        await _coursesRepository.Remove(request.Id, cancellationToken);

        /*
         * remove related assignments and enrollments
         */
        await _mediator.Publish(
            new CourseDeletedNotification(request.Id),
            cancellationToken);
    }
}