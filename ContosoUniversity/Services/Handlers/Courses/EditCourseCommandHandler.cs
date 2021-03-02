namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Domain;
    using Domain.Contracts;

    using MediatR;

    public class EditCourseCommandHandler : IRequestHandler<EditCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;

        public EditCourseCommandHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<Unit> Handle(EditCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);

            await _coursesRepository.Save(new Course(
                course.Code, //TODO: dirty, must be resolved
                request.Title,
                request.Credits,
                request.DepartmentId,
                request.Id));

            return Unit.Value;
        }
    }
}