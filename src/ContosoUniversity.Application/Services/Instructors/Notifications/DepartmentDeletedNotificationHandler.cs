namespace ContosoUniversity.Application.Services.Instructors.Notifications;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Departments.Notifications;

using Domain.Instructor;

using MediatR;

internal class DepartmentDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository) : INotificationHandler<DepartmentDeletedNotification>
{
    private readonly IInstructorsRwRepository _instructorsRepository = instructorsRepository;

    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Instructor[] instructors = await _instructorsRepository.GetAllAssignedToCourses(
            notification.CourseIds,
            cancellationToken);

        foreach (Instructor instructor in instructors)
        {
            foreach (Guid courseId in notification.CourseIds)
            {
                instructor.ResetCourseAssignment(courseId);
            }

            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}
