namespace ContosoUniversity.Services.Departments.Notifications;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Courses.Notifications;

using Domain.Contracts;
using Domain.Instructor;

using MediatR;

public class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
{
    private readonly IInstructorsRwRepository _instructorsRepository;

    public CourseDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Instructor[] instructors = await _instructorsRepository.GetAll(cancellationToken);

        foreach (Instructor instructor in instructors.Where(x => x.HasCourseAssigned(notification.Id)))
        {
            instructor.ResetCourseAssignment(notification.Id);
            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}