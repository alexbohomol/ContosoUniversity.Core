namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries.Courses;

    using ViewModels.Courses;

    public class CourseDetailsQueryHandler : IRequestHandler<CourseDetailsQuery, CourseDetailsViewModel>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public CourseDetailsQueryHandler(
            ICoursesRepository coursesRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;
        }

        public async Task<CourseDetailsViewModel> Handle(CourseDetailsQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            var department = await _departmentsRepository.GetById(course.DepartmentId);
            if (department == null)
                throw new EntityNotFoundException(nameof(department), course.DepartmentId);
            
            return new CourseDetailsViewModel
            {
                CourseCode = course.Code,
                Title = course.Title,
                Credits = course.Credits,
                Department = department.Name,
                Id = course.EntityId
            };
        }
    }
}