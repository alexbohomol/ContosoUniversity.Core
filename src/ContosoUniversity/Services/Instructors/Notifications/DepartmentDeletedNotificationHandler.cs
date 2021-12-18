namespace ContosoUniversity.Services.Instructors.Notifications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts;

using Departments.Notifications;

using Domain.Instructor;

using MediatR;

public class DepartmentDeletedNotificationHandler : INotificationHandler<DepartmentDeletedNotification>
{
    private readonly IInstructorsRwRepository _instructorsRepository;

    public DepartmentDeletedNotificationHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task Handle(DepartmentDeletedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Instructor[] instructors = await _instructorsRepository.GetAll(cancellationToken);

        IEnumerable<Instructor> assignedInstructors = instructors
            .Where(x => notification.CourseIds.Any(c => x.HasCourseAssigned(c)));

        foreach (Instructor instructor in assignedInstructors)
        {
            /*
             * Consider sending command here
             */
            foreach (Guid courseId in notification.CourseIds) instructor.ResetCourseAssignment(courseId);

            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}