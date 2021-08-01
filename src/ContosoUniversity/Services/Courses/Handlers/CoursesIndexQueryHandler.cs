namespace ContosoUniversity.Services.Courses.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using Queries;

    using ViewModels.Courses;

    public class CoursesIndexQueryHandler : IRequestHandler<CoursesIndexQuery, List<CourseListItemViewModel>>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public CoursesIndexQueryHandler(
            ICoursesRepository coursesRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;
        }

        public async Task<List<CourseListItemViewModel>> Handle(CoursesIndexQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesRepository.GetAll();

            var departmentNames = await _departmentsRepository.GetDepartmentNamesReference();

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames.Keys);

            return courses.Select(x => new CourseListItemViewModel
            {
                CourseCode = x.Code,
                Title = x.Title,
                Credits = x.Credits,
                Department = departmentNames[x.DepartmentId],
                Id = x.EntityId
            }).ToList();
        }
    }
}