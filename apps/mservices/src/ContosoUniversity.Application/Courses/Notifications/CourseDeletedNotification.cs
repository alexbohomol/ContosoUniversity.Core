namespace ContosoUniversity.Application.Courses.Notifications;

using System;

using MediatR;

public record CourseDeletedNotification(Guid Id) : INotification;
