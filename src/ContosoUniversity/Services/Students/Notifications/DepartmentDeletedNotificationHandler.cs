namespace ContosoUniversity.Services.Students.Notifications
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Departments.Notifications;

    using Domain.Contracts;

    using MediatR;

    public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
    {
        private readonly IStudentsRepository _studentsRepository;

        public DepartmentDeletedNotificationHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }
        
        public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
        {
            var students = await _studentsRepository.GetStudentsEnrolledForCourses(
                notification.CourseIds,
                cancellationToken);
            
            foreach (var student in students)
            {
                var withdrawIds = notification.CourseIds.Intersect(student.Enrollments.CourseIds).ToArray();
                student.WithdrawCourses(withdrawIds);
                await _studentsRepository.Save(student, cancellationToken);
            }
        }
    }
}