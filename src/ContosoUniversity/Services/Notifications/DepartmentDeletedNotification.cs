namespace ContosoUniversity.Services.Notifications
{
    using System;

    using MediatR;

    public record DepartmentDeletedNotification(Guid DepartmentId, Guid[] CourseIds) : INotification;
}