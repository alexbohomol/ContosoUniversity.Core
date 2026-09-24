namespace ContosoUniversity.SystemTests.InstructorsController;

using System;
using System.Collections.Generic;

using NUnit.Framework;

public record CreateInstructorRequest
{
    public string LastName { get; init; }
    public string FirstName { get; init; }
    public DateTime HireDate { get; init; }
    public Guid[] SelectedCourses { get; init; }
    public string Location { get; init; }

    public static readonly CreateInstructorRequest Valid = new()
    {
        LastName = "Bohomol",
        FirstName = "Alex",
        HireDate = new DateTime(2021, 12, 31),
        SelectedCourses = [],
        Location = "Campus"
    };

    public static IEnumerable<TestCaseData> Invalids =>
    [
        // LastName_is_too_short

        new TestCaseData(
                Valid with { LastName = new string('X', 51) },
                "The length of 'Last Name' must be 50 characters or fewer. You entered 51 characters.")
            .SetName("LastName_is_too_long"),

        // FirstName_is_too_short

        new TestCaseData(
                Valid with { FirstName = new string('X', 51) },
                "First name cannot be longer than 50 characters.")
            .SetName("FirstName_is_too_long")
    ];
}
