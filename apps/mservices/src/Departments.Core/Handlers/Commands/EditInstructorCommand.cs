namespace Departments.Core.Handlers.Commands;

using System;

using MediatR;

public record EditInstructorCommand(
    Guid ExternalId,
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location) : IRequest
{
    public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    public bool HasAssignedCourses =>
        SelectedCourses is not null
        && SelectedCourses.Length > 0;
}
