namespace ContosoUniversity.Application.Contracts.ReadModels;

using System;
using System.Collections.Generic;

using Domain;
using Domain.Student;

public record StudentReadModel(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Enrollment> Enrollments { get; }
    public string FullName => $"{FirstName}, {LastName}";
}