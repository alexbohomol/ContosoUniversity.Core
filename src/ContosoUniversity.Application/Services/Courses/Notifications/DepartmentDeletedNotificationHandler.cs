namespace ContosoUniversity.Application.Services.Courses.Notifications;

using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Departments.Notifications;

using MediatR;

internal class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
{
    private readonly ICoursesRwRepository _coursesRepository;

    public DepartmentDeletedNotificationHandler(ICoursesRwRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        await _coursesRepository.Remove(notification.CourseIds, cancellationToken);
    }
}
