using IStudentsRwRepository = Students.Core.IStudentsRwRepository;
using Student = Students.Core.Domain.Student;

namespace ContosoUniversity.Application.Students.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Courses.Notifications;

using MediatR;

internal class CourseDeletedNotificationHandler(IStudentsRwRepository studentsRepository)
    : INotificationHandler<CourseDeletedNotification>
{
    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        Guid[] courseIds = { notification.Id };

        Student[] enrolledStudents = await studentsRepository
            .GetStudentsEnrolledForCourses(courseIds, cancellationToken);

        if (enrolledStudents.Any())
        {
            foreach (Student student in enrolledStudents)
            {
                student.WithdrawCourses(courseIds);
                await studentsRepository.Save(student, cancellationToken);
            }
        }
    }
}
