namespace ContosoUniversity.Services.Instructors.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Departments.Notifications;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

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