namespace ContosoUniversity.Services.Handlers.Courses
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Data.Departments;
    using Data.Students;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class DeleteCourseCommandHandler : AsyncRequestHandler<DeleteCourseCommand>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;
        private readonly StudentsContext _studentsContext;

        public DeleteCourseCommandHandler(
            ICoursesRepository coursesRepository,
            DepartmentsContext departmentsContext,
            StudentsContext studentsContext)
        {
            _coursesRepository = coursesRepository;
            _departmentsContext = departmentsContext;
            _studentsContext = studentsContext;
        }

        protected override async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course is null)
                throw new FindException(
                    $"Could not find course with id:{request.Id}");

            /*
             * remove related assignments
             */
            var relatedAssignments = await _departmentsContext.CourseAssignments
                .Where(x => x.CourseExternalId == course.EntityId)
                .ToArrayAsync();
            _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);

            /*
             * remove related enrollments
             */
            var relatedEnrollments = await _studentsContext.Enrollments
                .Where(x => x.CourseExternalId == course.EntityId)
                .ToArrayAsync();
            _studentsContext.Enrollments.RemoveRange(relatedEnrollments);

            await _departmentsContext.SaveChangesAsync();
            await _studentsContext.SaveChangesAsync();
            await _coursesRepository.Remove(course.EntityId);
        }
    }
}