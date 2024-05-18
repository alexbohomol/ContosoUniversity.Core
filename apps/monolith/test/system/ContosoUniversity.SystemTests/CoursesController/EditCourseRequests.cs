namespace ContosoUniversity.SystemTests.CoursesController;

using System;
using System.Collections.Generic;

using Mvc.ViewModels.Courses;

using NUnit.Framework;

public static class EditCourseRequests
{
    public static EditCourseRequest Valid = new()
    {
        Title = "Computers Algebra",
        Credits = 3,
        DepartmentId = new Guid("72c0804d-b208-4e67-82ba-cf54dc93dcc8")
    };

    public static IEnumerable<TestCaseData> Invalids =>
    [
        new TestCaseData(
            Valid with { Title = "#@" },
            "The field 'Title' must be a string with a minimum length of 3 and a maximum length of 50."),
        new TestCaseData(
            Valid with { Title = "123456789012345678901234567890123456789012345678901" },
            "The field 'Title' must be a string with a minimum length of 3 and a maximum length of 50."),
        new TestCaseData(
            Valid with { Credits = -1 },
            "The field 'Credits' must be between 0 and 5."),
        new TestCaseData(
            Valid with { Credits = 6 },
            "The field 'Credits' must be between 0 and 5."),

        //     "The DepartmentId field is required.",
    ];
}
