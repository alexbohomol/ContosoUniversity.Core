namespace ContosoUniversity.Services.Courses.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries;

    using ViewModels.Courses;

    public class CourseEditFormQueryHandler : IRequestHandler<CourseEditFormQuery, CourseEditForm>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public CourseEditFormQueryHandler(
            ICoursesRepository coursesRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;
        }

        public async Task<CourseEditForm> Handle(CourseEditFormQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            var departments = await _departmentsRepository.GetDepartmentNamesReference();

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(
                new [] { course }, 
                departments.Keys);

            return new CourseEditForm(
                new EditCourseCommand(course),
                course.Code,
                departments);
        }
    }
}