namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Courses;
    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Courses;

    using ViewModels.Courses;

    public class GetCourseDetailsHandler : IRequestHandler<GetCourseDetailsQuery, CourseDetailsViewModel>
    {
        private readonly CoursesContext _coursesContext;
        private readonly DepartmentsContext _departmentsContext;

        public GetCourseDetailsHandler(
            CoursesContext coursesContext,
            DepartmentsContext departmentsContext)
        {
            _coursesContext = coursesContext;
            _departmentsContext = departmentsContext;
        }

        public async Task<CourseDetailsViewModel> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesContext.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == request.Id);
            if (course == null)
            {
                return null;
            }

            var department = await _departmentsContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == course.DepartmentExternalId);

            /*
             * TODO: missing context boundary check when department is null
             */

            return new CourseDetailsViewModel
            {
                CourseCode = course.CourseCode,
                Title = course.Title,
                Credits = course.Credits,
                Department = department.Name,
                Id = course.ExternalId
            };
        }
    }
}