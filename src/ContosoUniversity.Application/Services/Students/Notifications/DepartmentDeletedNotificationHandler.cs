namespace ContosoUniversity.Application.Services.Students.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Departments.Notifications;

using Domain.Student;

using MediatR;

public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
{
    private readonly IStudentsRwRepository _studentsRepository;

    public DepartmentDeletedNotificationHandler(IStudentsRwRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        Student[] students = await _studentsRepository.GetStudentsEnrolledForCourses(
            notification.CourseIds,
            cancellationToken);

        foreach (Student student in students)
        {
            Guid[] withdrawIds = notification.CourseIds
                .Intersect(student.Enrollments.Select(x => x.CourseId))
                .ToArray();
            student.WithdrawCourses(withdrawIds);
            await _studentsRepository.Save(student, cancellationToken);
        }
    }
}