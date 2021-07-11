namespace ContosoUniversity.Services.Events
{
    using System;

    using MediatR;

    public record DepartmentDeleted(Guid DepartmentId, Guid[] CourseIds) : INotification;
}