namespace ContosoUniversity.SystemTests.StudentsController;

using System;
using System.Collections.Generic;

using Mvc.ViewModels.Students;

using NUnit.Framework;

public static class CreateStudentRequests
{
    public static readonly CreateStudentRequest Valid = new()
    {
        LastName = "Alexander",
        FirstName = "Alpert",
        EnrollmentDate = DateTime.Parse("2022-01-17")
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
