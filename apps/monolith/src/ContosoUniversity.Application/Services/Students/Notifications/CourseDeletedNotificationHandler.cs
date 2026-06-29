namespace ContosoUniversity.Application.Services.Students.Notifications;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.Writes;

using Courses.Notifications;

using Domain.Student;

using MediatR;

internal class CourseDeletedNotificationHandler(IStudentsRwRepository studentsRepository)
    : INotificationHandler<CourseDeletedNotification>
{
    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        Guid[] courseIds = { notification.Id };

        Student[] enrolledStudents = await studentsRepository
            .GetStudentsEnrolledForCourses(courseIds, cancellationToken);

        if (enrolledStudents.Length != 0)
        {
            foreach (Student student in enrolledStudents)
            {
                student.WithdrawCourses(courseIds);
                await studentsRepository.Save(student, cancellationToken);
            }
        }
    }
}
