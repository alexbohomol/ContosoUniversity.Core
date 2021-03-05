namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using ViewModels.Courses;

    public class CreateCourseHandler : IRequestHandler<CourseCreateForm>
    {
        private readonly ICoursesRepository _coursesRepository;

        public CreateCourseHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<Unit> Handle(CourseCreateForm request, CancellationToken cancellationToken)
        {
            await _coursesRepository.Save(new Domain.Course(
                request.CourseCode,
                request.Title,
                request.Credits,
                request.DepartmentId));
            
            return Unit.Value;
        }
    }
}