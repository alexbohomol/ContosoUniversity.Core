namespace ContosoUniversity.Services.Courses.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using Notifications;

    public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
    {
        private readonly ICoursesRepository _coursesRepository;

        public DepartmentDeletedNotificationHandler(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }
        
        public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
        {
            await _coursesRepository.Remove(notification.CourseIds);
        }
    }
}