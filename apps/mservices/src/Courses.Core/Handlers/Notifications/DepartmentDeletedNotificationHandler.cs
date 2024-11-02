namespace Courses.Core.Handlers.Notifications;

using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MediatR;

internal class DepartmentDeletedNotificationHandler(ICoursesRwRepository coursesRepository)
    : INotificationHandler<DepartmentDeletedNotification>
{
    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        await coursesRepository.Remove(notification.CourseIds, cancellationToken);
    }
}
