namespace ContosoUniversity.Services.Handlers.Instructors
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Notifications;

    public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
    {
        private readonly DepartmentsContext _departmentsContext;

        public DepartmentDeletedNotificationHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
        {
            var relatedAssignments = await _departmentsContext.CourseAssignments
                .Where(x => notification.CourseIds.Contains(x.CourseExternalId))
                .ToArrayAsync(cancellationToken: cancellationToken);
            
            _departmentsContext.CourseAssignments.RemoveRange(relatedAssignments);

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}