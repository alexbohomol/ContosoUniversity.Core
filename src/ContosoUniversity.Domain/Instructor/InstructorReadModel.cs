namespace ContosoUniversity.Domain.Instructor;

using System;
using System.Collections.Generic;
using System.Linq;

public record InstructorReadModel(
    string FirstName,
    string LastName,
    DateTime HireDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Guid> Courses => _courseAssignments?
        .Select(x => x.CourseId)
        .ToList();

    public string Office => _officeAssignment?.Title;

    public string FullName => $"{LastName}, {FirstName}";

#pragma warning disable CS0649
    private IList<CourseAssignment> _courseAssignments;
    private OfficeAssignment _officeAssignment;
#pragma warning restore CS0649
}