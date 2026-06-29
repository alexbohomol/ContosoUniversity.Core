namespace ContosoUniversity.Application.Contracts.Repositories.Reads.Projections;

using System;
using System.Collections.Generic;

using ContosoUniversity.Domain;
using ContosoUniversity.Domain.Student;

public record Student(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Enrollment> Enrollments { get; }
    public string FullName => $"{FirstName}, {LastName}";
}
