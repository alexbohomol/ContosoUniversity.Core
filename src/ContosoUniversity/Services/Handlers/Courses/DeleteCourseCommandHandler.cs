namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using Notifications;

    using MediatR;

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
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            await _coursesRepository.Remove(course.EntityId);

            /*
             * remove related assignments and enrollments
             */
            await _mediator.Publish(
                new CourseDeletedNotification(request.Id),
                cancellationToken);
        }
    }
}