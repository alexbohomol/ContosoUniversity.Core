namespace ContosoUniversity.Domain.Student;

using System;
using System.Collections.Generic;

public record StudentReadModel(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Enrollment> Enrollments { get; }
    public string FullName => $"{FirstName}, {LastName}";
}