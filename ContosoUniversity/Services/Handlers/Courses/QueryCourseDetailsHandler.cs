namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Courses;

    using ViewModels.Courses;

    public class QueryCourseDetailsHandler : IRequestHandler<QueryCourseDetails, CourseDetailsViewModel>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;

        public QueryCourseDetailsHandler(
            ICoursesRepository coursesRepository,
            DepartmentsContext departmentsContext)
        {
            _coursesRepository = coursesRepository;
            _departmentsContext = departmentsContext;
        }

        public async Task<CourseDetailsViewModel> Handle(QueryCourseDetails request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
            {
                return null;
            }

            var department = await _departmentsContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == course.DepartmentId);

            /*
             * TODO: missing context boundary check when department is null
             */

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