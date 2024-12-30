namespace ContosoUniversity.SystemTests.StudentsController;

using System;
using System.Collections.Generic;

using NUnit.Framework;

public record EditStudentRequest
{
    public DateTime EnrollmentDate { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }
    public Guid ExternalId { get; init; }
}

public static class EditStudentRequests
{
    public static readonly EditStudentRequest Valid = new()
    {
        FirstName = "Cupertino",
        LastName = "Alex",
        EnrollmentDate = DateTime.Parse("2022-01-31")
    };

    public static IEnumerable<TestCaseData> Invalids =>
    [
        new TestCaseData(
            Valid with { LastName = "123456789012345678901234567890123456789012345678901" },
            "Last name cannot be longer than 50 characters."),
        new TestCaseData(
            Valid with { FirstName = "123456789012345678901234567890123456789012345678901" },
            "First name cannot be longer than 50 characters."),
    ];
}
