namespace ContosoUniversity.Services.Events
{
    using System;

    using MediatR;

    public record CourseDeletedNotification(Guid Id) : INotification;
}