namespace ContosoUniversity.Services.Events
{
    using System;

    using MediatR;

    public record CourseDeleted(Guid Id) : INotification;
}