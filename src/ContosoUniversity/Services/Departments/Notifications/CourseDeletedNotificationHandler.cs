namespace ContosoUniversity.Services.Departments.Notifications;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Courses.Notifications;

using Domain.Contracts;
using Domain.Instructor;

using MediatR;

public class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
{
    private readonly IInstructorsRwRepository _instructorsRepository;

    public CourseDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        IEnumerable<Instructor> instructors = (await _instructorsRepository.GetAll(cancellationToken))
            .Where(x => x.Courses.Contains(notification.Id));

        foreach (Instructor instructor in instructors)
        {
            instructor.Courses.Remove(notification.Id);
            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}