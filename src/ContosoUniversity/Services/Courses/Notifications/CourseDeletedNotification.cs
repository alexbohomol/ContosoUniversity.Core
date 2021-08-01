namespace ContosoUniversity.Services.Courses.Notifications
{
    using System;

    using MediatR;

    public record CourseDeletedNotification(Guid Id) : INotification;
}