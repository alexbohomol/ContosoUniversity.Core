using Student = Students.Core.Domain.Student;

namespace Students.Core.Handlers.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MediatR;

internal class DepartmentDeletedNotificationHandler(IStudentsRwRepository studentsRepository)
    : INotificationHandler<DepartmentDeletedNotification>
{
    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        Student[] students = await studentsRepository.GetStudentsEnrolledForCourses(
            notification.CourseIds,
            cancellationToken);

        foreach (Student student in students)
        {
            Guid[] withdrawIds = notification.CourseIds
                .Intersect(student.Enrollments.Select(x => x.CourseId))
                .ToArray();
            student.WithdrawCourses(withdrawIds);
            await studentsRepository.Save(student, cancellationToken);
        }
    }
}
