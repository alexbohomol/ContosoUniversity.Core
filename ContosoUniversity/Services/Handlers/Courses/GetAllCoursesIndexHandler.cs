namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Courses;
    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Courses;

    using ViewModels.Courses;

    public class GetAllCoursesIndexHandler : IRequestHandler<GetCoursesIndexQuery, List<CourseListItemViewModel>>
    {
        private readonly CoursesContext _coursesContext;
        private readonly DepartmentsContext _departmentsContext;

        public GetAllCoursesIndexHandler(
            CoursesContext coursesContext,
            DepartmentsContext departmentsContext)
        {
            _coursesContext = coursesContext;
            _departmentsContext = departmentsContext;
        }

        public async Task<List<CourseListItemViewModel>> Handle(GetCoursesIndexQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesContext.Courses.AsNoTracking().ToListAsync();

            var departmentNames = await _departmentsContext.Departments
                .Where(x => courses.Select(_ => _.DepartmentExternalId).Distinct().Contains(x.ExternalId))
                .AsNoTracking()
                .ToDictionaryAsync(x => x.ExternalId, x => x.Name);

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(courses, departmentNames);

            return courses.Select(x => new CourseListItemViewModel
            {
                CourseCode = x.CourseCode,
                Title = x.Title,
                Credits = x.Credits,
                Department = departmentNames[x.DepartmentExternalId],
                Id = x.ExternalId
            }).ToList();
        }
    }
}