namespace ContosoUniversity.Services.Courses.Commands
{
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
            var course = await _coursesRepository.GetById(request.Id, cancellationToken);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            await _coursesRepository.Remove(course.EntityId, cancellationToken);

            /*
             * remove related assignments and enrollments
             */
            await _mediator.Publish(
                new CourseDeletedNotification(request.Id),
                cancellationToken);
        }
    }
}