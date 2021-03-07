namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Students;

    using Events;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class CourseDeletedHandler : INotificationHandler<CourseDeleted>
    {
        private readonly StudentsContext _studentsContext;

        public CourseDeletedHandler(StudentsContext studentsContext)
        {
            _studentsContext = studentsContext;
        }

        public async Task Handle(CourseDeleted notification, CancellationToken cancellationToken)
        {
            var enrollments = await _studentsContext
                .Enrollments
                .Where(x => x.CourseExternalId == notification.Id)
                .ToArrayAsync(cancellationToken: cancellationToken);

            if (enrollments.Any())
            {
                _studentsContext.Enrollments.RemoveRange(enrollments);

                await _studentsContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}