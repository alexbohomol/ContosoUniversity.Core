using Instructor = Departments.Core.Domain.Instructor;

namespace Departments.Core.Handlers.Notifications;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MediatR;

internal class DepartmentDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    : INotificationHandler<DepartmentDeletedNotification>
{
    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Instructor[] instructors = await instructorsRepository.GetAllAssignedToCourses(
            notification.CourseIds,
            cancellationToken);

        foreach (Instructor instructor in instructors)
        {
            foreach (Guid courseId in notification.CourseIds)
            {
                instructor.ResetCourseAssignment(courseId);
            }

            await instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}
