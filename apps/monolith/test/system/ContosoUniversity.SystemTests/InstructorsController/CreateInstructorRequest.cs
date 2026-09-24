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

    public static IEnumerable<TestCaseData> ValidInstructorVariants =>
    [
        new TestCaseData(Valid)
            .SetName("Without_Administrator"),
        new TestCaseData(Valid)
            .SetName("With_Administrator")
    ];

    public static IEnumerable<TestCaseData> Invalids =>
    [
        new TestCaseData(
                Valid with { LastName = "XY" },
                "'LastName' must be between 3 and 50 characters. You entered 2 characters.")
            .SetName("LastName_is_too_short"),
        new TestCaseData(
                Valid with { LastName = new string('X', 51) },
                "'LastName' must be between 3 and 50 characters. You entered 51 characters.")
            .SetName("LastName_is_too_long"),
        new TestCaseData(
                Valid with { FirstName = "XY" },
                "'FirstName' must be between 3 and 50 characters. You entered 2 characters.")
            .SetName("FirstName_is_too_short"),
        new TestCaseData(
                Valid with { FirstName = new string('X', 51) },
                "'FirstName' must be between 3 and 50 characters. You entered 51 characters.")
            .SetName("FirstName_is_too_long")
    ];
}
