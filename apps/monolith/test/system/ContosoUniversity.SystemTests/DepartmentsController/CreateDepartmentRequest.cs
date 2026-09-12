namespace ContosoUniversity.SystemTests.DepartmentsController;

using System;
using System.Collections.Generic;

using NUnit.Framework;

public record CreateDepartmentRequest
{
    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public string AdministratorName { get; init; }

    public static readonly CreateDepartmentRequest Valid = new()
    {
        Name = "Informatics",
        Budget = 1000000.00m,
        StartDate = new DateTime(2021, 9, 1),
        AdministratorName = "Zheng, Roger"
    };

    public static IEnumerable<TestCaseData> Invalids =>
    [
        new TestCaseData(
            Valid with { Name = "XY" },
            "'Name' must be between 3 and 50 characters. You entered 2 characters."),
        new TestCaseData(
            Valid with { Name = new string('X', 51) },
            "'Name' must be between 3 and 50 characters. You entered 51 characters.")
    ];
}
