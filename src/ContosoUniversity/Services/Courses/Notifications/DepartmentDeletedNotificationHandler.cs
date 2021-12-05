namespace ContosoUniversity.Services.Courses.Notifications;

using System.Threading;
using System.Threading.Tasks;

using Departments.Notifications;

using Domain.Contracts;

using MediatR;

public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
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