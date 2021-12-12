namespace ContosoUniversity.Services.Instructors.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Departments.Notifications;

using Domain.Contracts;
using Domain.Instructor;

using MediatR;

public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
{
    private readonly IInstructorsRwRepository _instructorsRepository;

    public DepartmentDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        Instructor[] instructors = (await _instructorsRepository.GetAll(cancellationToken))
            .Where(x => x.Courses.Any(c => notification.CourseIds.Contains(c)))
            .ToArray();

        foreach (Instructor instructor in instructors)
        {
            foreach (Guid courseId in notification.CourseIds) instructor.Courses.Remove(courseId);
            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}