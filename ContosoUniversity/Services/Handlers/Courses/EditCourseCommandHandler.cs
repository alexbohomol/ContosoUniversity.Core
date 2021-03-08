namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    public class EditCourseCommandHandler : AsyncRequestHandler<EditCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;

        public EditCourseCommandHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        protected override async Task Handle(EditCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            course.Update(
                request.Title,
                request.Credits,
                request.DepartmentId);

            await _coursesRepository.Save(course);
        }
    }
}