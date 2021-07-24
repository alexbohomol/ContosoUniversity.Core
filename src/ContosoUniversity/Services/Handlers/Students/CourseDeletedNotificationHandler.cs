namespace ContosoUniversity.Services.Handlers.Students
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using Notifications;

    using MediatR;

    public class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
    {
        private readonly IStudentsRepository _studentsRepository;

        public CourseDeletedNotificationHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
        {
            Guid[] courseIds = { notification.Id };
            
            var enrolledStudents = await _studentsRepository.GetStudentsEnrolledForCourses(courseIds);
            if (enrolledStudents.Any())
            {
                foreach (var student in enrolledStudents)
                {
                    student.WithdrawCourses(courseIds);
                    await _studentsRepository.Save(student);
                }
            }
        }
    }
}