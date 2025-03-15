namespace ContosoUniversity.Messaging.Contracts;

using System;

using MassTransit;

[EntityName("course-deleted-event")]
public record CourseDeletedEvent(Guid Id);
