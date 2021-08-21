namespace ContosoUniversity.Services.Instructors.Notifications
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Departments.Notifications;

    using Domain.Contracts;

    using MediatR;

    public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
    {
        private readonly IInstructorsRepository _instructorsRepository;

        public DepartmentDeletedNotificationHandler(IInstructorsRepository instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }
        
        public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
        {
            var instructors = (await _instructorsRepository.GetAll())
                .Where(x => x.Courses.Any(c => notification.CourseIds.Contains(c)))
                .ToArray();

            foreach (var instructor in instructors)
            {
                foreach (var courseId in notification.CourseIds)
                {
                    instructor.Courses.Remove(courseId);
                }
                await _instructorsRepository.Save(instructor);
            }
        }
    }
}