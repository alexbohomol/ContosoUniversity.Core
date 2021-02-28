namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Courses;
    using Data.Courses.Models;

    using MediatR;

    using ViewModels.Courses;

    public class CreateCourseHandler : IRequestHandler<CourseCreateForm>
    {
        private readonly CoursesContext _coursesContext;

        public CreateCourseHandler(CoursesContext coursesContext)
        {
            _coursesContext = coursesContext;
        }

        public async Task<Unit> Handle(CourseCreateForm request, CancellationToken cancellationToken)
        {
            // form.ToDomainModel()
            var course = Domain.Course.Create(
                request.CourseCode,
                request.Title,
                request.Credits,
                request.DepartmentId);

            // domain.ToDataModel()
            var entity = new Course
            {
                CourseCode = course.Code,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentExternalId = course.DepartmentId,
                ExternalId = course.ExternalId
            };
            
            _coursesContext.Add(entity);
            
            await _coursesContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}