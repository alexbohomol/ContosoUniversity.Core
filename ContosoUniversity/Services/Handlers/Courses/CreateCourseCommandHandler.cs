namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Domain;
    using Domain.Contracts;

    using MediatR;

    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;

        public CreateCourseCommandHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<Unit> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            await _coursesRepository.Save(new Course(
                request.CourseCode,
                request.Title,
                request.Credits,
                request.DepartmentId));

            return default;
        }
    }
}