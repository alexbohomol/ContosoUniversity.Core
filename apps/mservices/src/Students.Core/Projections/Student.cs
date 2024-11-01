namespace Students.Core.Projections;

using System;
using System.Collections.Generic;

using ContosoUniversity.Domain;

using Domain;

public record Student(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Enrollment> Enrollments { get; }
    public string FullName => $"{FirstName}, {LastName}";
}
