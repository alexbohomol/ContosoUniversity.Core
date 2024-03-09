namespace ContosoUniversity.Application.Services.Courses.Notifications;

using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Departments.Notifications;

using MediatR;

internal class DepartmentDeletedNotificationHandler(ICoursesRwRepository coursesRepository) : INotificationHandler<DepartmentDeletedNotification>
{
    private readonly ICoursesRwRepository _coursesRepository = coursesRepository;

    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        await _coursesRepository.Remove(notification.CourseIds, cancellationToken);
    }
}
