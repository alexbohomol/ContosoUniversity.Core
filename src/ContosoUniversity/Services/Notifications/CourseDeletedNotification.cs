namespace ContosoUniversity.Services.Notifications
{
    using System;

    using MediatR;

    public record CourseDeletedNotification(Guid Id) : INotification;
}