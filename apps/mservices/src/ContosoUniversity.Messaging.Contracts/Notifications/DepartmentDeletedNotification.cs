namespace ContosoUniversity.Messaging.Contracts.Notifications;

using System;

using MediatR;

public record DepartmentDeletedNotification(Guid[] CourseIds) : INotification;
