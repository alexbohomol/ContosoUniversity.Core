namespace ContosoUniversity.Application.Services.Students.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Departments.Notifications;

using global::Students.Core;
using global::Students.Core.Domain;

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
