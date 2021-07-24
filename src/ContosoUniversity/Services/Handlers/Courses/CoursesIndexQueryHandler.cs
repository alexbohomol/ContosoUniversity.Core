namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Courses;

    using ViewModels.Courses;

    public class CoursesIndexQueryHandler : IRequestHandler<CoursesIndexQuery, List<CourseListItemViewModel>>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;

        public CoursesIndexQueryHandler(
            ICoursesRepository coursesRepository,
            DepartmentsContext departmentsContext)
        {
            _coursesRepository = coursesRepository;
            _departmentsContext = departmentsContext;
        }

        public async Task<List<CourseListItemViewModel>> Handle(CoursesIndexQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesRepository.GetAll();

            var departmentNames = await _departmentsContext.Departments
                .Where(x => courses.Select(_ => _.DepartmentId).Distinct().Contains(x.ExternalId))
                .AsNoTracking()
                .ToDictionaryAsync(x => x.ExternalId, x => x.Name, cancellationToken);

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