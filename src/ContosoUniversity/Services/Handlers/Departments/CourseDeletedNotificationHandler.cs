namespace ContosoUniversity.Services.Handlers.Departments
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Events;

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