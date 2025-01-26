namespace Departments.Core.Handlers.Commands;

using System;

using Domain;

using MediatR;

public record CreateInstructorCommand(
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location) : IRequest<Instructor>
{
    public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    public bool HasAssignedCourses =>
        SelectedCourses is not null
        && SelectedCourses.Length > 0;
}
