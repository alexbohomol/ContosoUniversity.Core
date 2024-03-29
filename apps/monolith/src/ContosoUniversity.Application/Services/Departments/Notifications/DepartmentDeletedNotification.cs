namespace ContosoUniversity.Application.Services.Departments.Notifications;

using System;

using MediatR;

public record DepartmentDeletedNotification(Guid[] CourseIds) : INotification;
