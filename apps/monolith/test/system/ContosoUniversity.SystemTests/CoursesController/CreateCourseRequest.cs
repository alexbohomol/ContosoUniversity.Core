namespace ContosoUniversity.SystemTests.CoursesController;

using System;
using System.Collections.Generic;

using NUnit.Framework;

public record CreateCourseRequest
{
    public int CourseCode { get; init; }
    public string Title { get; init; }
    public int Credits { get; init; }
    public Guid DepartmentId { get; init; }

    public static readonly CreateCourseRequest Valid = new()
    {
        CourseCode = 1111,
        Title = "Computers",
        Credits = 5,
        DepartmentId = new Guid("dab7e678-e3e7-4471-8282-96fe52e5c16f")
    };

    public static IEnumerable<TestCaseData> Invalids =>
    [
        new TestCaseData(
            Valid with { CourseCode = 999 },
            "Course code can have a value from 1000 to 9999."),
        new TestCaseData(
            Valid with { CourseCode = 10000 },
            "Course code can have a value from 1000 to 9999."),
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
