namespace ContosoUniversity.Application.Services.Courses.Notifications;

using System.Threading;
using System.Threading.Tasks;

using Departments.Notifications;

using global::Courses.Core;

using MediatR;

internal class DepartmentDeletedNotificationHandler(ICoursesRwRepository coursesRepository)
    : INotificationHandler<DepartmentDeletedNotification>
{
    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        await coursesRepository.Remove(notification.CourseIds, cancellationToken);
    }
}
