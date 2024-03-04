namespace ContosoUniversity.Application.Services.Departments.Notifications;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Courses.Notifications;

using Domain.Instructor;

using MediatR;

internal class CourseDeletedNotificationHandler : INotificationHandler<CourseDeletedNotification>
{
    private readonly IInstructorsRwRepository _instructorsRepository;

    public CourseDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task Handle(CourseDeletedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Instructor[] instructors = await _instructorsRepository.GetAllAssignedToCourses(
            new[] { notification.Id },
            cancellationToken);

        foreach (Instructor instructor in instructors)
        {
            instructor.ResetCourseAssignment(notification.Id);
            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}
