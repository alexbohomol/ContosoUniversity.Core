namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using Events;

    using MediatR;

    public class DepartmentDeletedHandler : INotificationHandler<DepartmentDeleted>
    {
        private readonly IStudentsRepository _studentsRepository;

        public DepartmentDeletedHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }
        
        public async Task Handle(DepartmentDeleted notification, CancellationToken cancellationToken)
        {
            var students = await _studentsRepository.GetStudentsEnrolledForCourses(notification.CourseIds);
            foreach (var student in students)
            {
                var withdrawIds = notification.CourseIds.Intersect(student.Enrollments.CourseIds).ToArray();
                student.WithdrawCourses(withdrawIds);
                await _studentsRepository.Save(student);
            }
        }
    }
}