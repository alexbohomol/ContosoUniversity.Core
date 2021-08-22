namespace ContosoUniversity.Services.Departments.Notifications
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Courses.Notifications;

    using Domain.Contracts;

    using MediatR;

    public class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
    {
        private readonly IInstructorsRepository _instructorsRepository;

        public CourseDeletedNotificationHandler(IInstructorsRepository instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }

        public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
        {
            var instructors = (await _instructorsRepository.GetAll(cancellationToken))
                .Where(x => x.Courses.Contains(notification.Id));

            foreach (var instructor in instructors)
            {
                instructor.Courses.Remove(notification.Id);
                await _instructorsRepository.Save(instructor, cancellationToken);
            }
        }
    }
}