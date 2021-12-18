namespace ContosoUniversity.Services.Students.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Courses.Notifications;

using Domain.Student;

using MediatR;

public class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
{
    private readonly IStudentsRwRepository _studentsRepository;

    public CourseDeletedNotificationHandler(IStudentsRwRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        Guid[] courseIds = { notification.Id };

        Student[] enrolledStudents = await _studentsRepository
            .GetStudentsEnrolledForCourses(courseIds, cancellationToken);

        if (enrolledStudents.Any())
            foreach (Student student in enrolledStudents)
            {
                student.WithdrawCourses(courseIds);
                await _studentsRepository.Save(student, cancellationToken);
            }
    }
}