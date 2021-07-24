namespace ContosoUniversity.Services.Events
{
    using System;

    using MediatR;

    public record DepartmentDeletedNotification(Guid DepartmentId, Guid[] CourseIds) : INotification;
}