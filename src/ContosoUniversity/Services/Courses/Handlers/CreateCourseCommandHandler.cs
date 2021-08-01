namespace ContosoUniversity.Services.Courses.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;
    using Domain.Course;

    using MediatR;

    public class CreateCourseCommandHandler : AsyncRequestHandler<CreateCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;

        public CreateCourseCommandHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        protected override Task Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            return _coursesRepository.Save(
                new Course(
                    request.CourseCode,
                    request.Title,
                    request.Credits,
                    request.DepartmentId));
        }
    }
}