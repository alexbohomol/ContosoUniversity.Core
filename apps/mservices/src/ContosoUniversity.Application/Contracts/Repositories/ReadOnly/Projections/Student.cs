namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Projections;

using System;
using System.Collections.Generic;

using Domain;
using Domain.Student;

public record Student(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Enrollment> Enrollments { get; }
    public string FullName => $"{FirstName}, {LastName}";
}
