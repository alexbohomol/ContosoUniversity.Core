namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class DeleteDepartmentCommandHandler : AsyncRequestHandler<DeleteDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IStudentsRepository _studentsRepository;

        public DeleteDepartmentCommandHandler(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository,
            IStudentsRepository studentsRepository)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
            _studentsRepository = studentsRepository;
        }
        
        protected override async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsContext.Departments
                .FirstOrDefaultAsync(x => x.ExternalId == request.Id, cancellationToken: cancellationToken);
            
            if (department != null)
            {
                var relatedCourses = await _coursesRepository.GetByDepartmentId(request.Id);
                var relatedCoursesIds = relatedCourses.Select(x => x.EntityId).ToArray();

                /*
                 * remove related assignments
                 */
                var relatedAssignments = await _departmentsContext.CourseAssignments
                    .Where(x => relatedCoursesIds.Contains(x.CourseExternalId))
                    .ToArrayAsync(cancellationToken: cancellationToken);
                _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);

                /*
                 * remove related enrollments (withdraw related students)
                 */
                var students = await _studentsRepository.GetStudentsEnrolledForCourses(relatedCoursesIds);
                foreach (var student in students)
                {
                    var withdrawIds = relatedCoursesIds.Intersect(student.Enrollments.CourseIds);
                    student.WithdrawCourses(withdrawIds.ToArray());
                    await _studentsRepository.Save(student);
                }

                /*
                 * remove related courses
                 */
                await _coursesRepository.Remove(relatedCoursesIds);

                /*
                 * remove department
                 */
                _departmentsContext.Departments.Remove(department);

                await _departmentsContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}