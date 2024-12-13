using Instructor = Departments.Core.Domain.Instructor;

namespace Departments.Core.Handlers.Notifications;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MediatR;

internal class CourseDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    : INotificationHandler<CourseDeletedNotification>
{
    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Instructor[] instructors = await instructorsRepository.GetAllAssignedToCourses(
            new[] { notification.Id },
            cancellationToken);

        foreach (Instructor instructor in instructors)
        {
            instructor.ResetCourseAssignment(notification.Id);
            await instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}
