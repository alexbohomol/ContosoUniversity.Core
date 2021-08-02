namespace ContosoUniversity.Services.Departments.Notifications
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Courses.Notifications;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
    {
        private readonly DepartmentsContext _departmentsContext;

        public CourseDeletedNotificationHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }

        public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
        {
            var assignments = await _departmentsContext
                .CourseAssignments
                .Where(x => x.CourseExternalId == notification.Id)
                .ToArrayAsync(cancellationToken);

            if (assignments.Any())
            {
                _departmentsContext.CourseAssignments.RemoveRange(assignments);
            
                await _departmentsContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}