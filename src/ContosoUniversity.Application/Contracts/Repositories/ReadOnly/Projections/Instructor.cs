namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Projections;

using System;
using System.Collections.Generic;
using System.Linq;

using Domain;
using Domain.Instructor;

public record Instructor(
    string FirstName,
    string LastName,
    DateTime HireDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public Guid[] Courses => _courseAssignments?
        .Select(x => x.CourseId)
        .ToArray();

    public string Office => _officeAssignment?.Title;

#pragma warning disable CS0649
    private IList<CourseAssignment> _courseAssignments;
    private OfficeAssignment _officeAssignment;
#pragma warning restore CS0649
}