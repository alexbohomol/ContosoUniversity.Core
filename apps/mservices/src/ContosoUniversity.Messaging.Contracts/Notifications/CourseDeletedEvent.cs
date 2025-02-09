namespace ContosoUniversity.Messaging.Contracts.Notifications;

using System;

using MassTransit;

[EntityName("course-deleted-event")]
public record CourseDeletedEvent(Guid Id);
