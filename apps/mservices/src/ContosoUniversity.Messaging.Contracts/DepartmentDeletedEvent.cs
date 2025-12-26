namespace ContosoUniversity.Messaging.Contracts;

using System;

using MassTransit;

[EntityName("department-deleted-event")]
public record DepartmentDeletedEvent(Guid DepartmentId);
