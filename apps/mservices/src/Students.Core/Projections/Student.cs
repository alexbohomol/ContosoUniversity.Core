namespace Students.Core.Projections;

using System;
using System.Collections.Generic;

using ContosoUniversity.SharedKernel;

using Domain;

public record Student(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public IList<Enrollment> Enrollments { get; }
}
