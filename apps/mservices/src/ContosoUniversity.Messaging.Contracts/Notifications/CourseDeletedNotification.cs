namespace ContosoUniversity.Messaging.Contracts.Notifications;

using System;

using MediatR;

public record CourseDeletedNotification(Guid Id) : INotification;
