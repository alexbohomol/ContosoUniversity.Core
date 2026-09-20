namespace ContosoUniversity.SystemTests.DepartmentsController;

using System;
using System.Collections.Generic;

using NUnit.Framework;

public record EditDepartmentRequest
{
    public string Name { get; init; }
    public decimal Budget { get; init; }
    public DateTime StartDate { get; init; }
    public string AdministratorName { get; init; }

    public static readonly EditDepartmentRequest Valid = new()
    {
        Name = "Computers",
        Budget = 1100000.00m,
        StartDate = new DateTime(2022, 9, 1),
        AdministratorName = "Harui, Roger"
    };

    public static IEnumerable<TestCaseData> Invalids =>
    [
        new TestCaseData(
            Valid with { Name = "XY" },
            "'Name' must be between 3 and 50 characters. You entered 2 characters.")
            .SetName("Name_is_too_short"),
        new TestCaseData(
            Valid with { Name = new string('X', 51) },
            "'Name' must be between 3 and 50 characters. You entered 51 characters.")
            .SetName("Name_is_too_long")
    ];

    public static IEnumerable<TestCaseData> AdministratorTransitions =>
    [
        new TestCaseData(
            CreateDepartmentRequest.Valid with { AdministratorName = null },
            Valid with { AdministratorName = "Zheng, Roger" })
            .SetName("Unset_to_Zheng_Roger"),
        new TestCaseData(
            CreateDepartmentRequest.Valid with { AdministratorName = "Zheng, Roger" },
            Valid with { AdministratorName = "Harui, Roger" })
            .SetName("Zheng_Roger_to_Harui_Roger"),
        new TestCaseData(
            CreateDepartmentRequest.Valid with { AdministratorName = "Zheng, Roger" },
            Valid with { AdministratorName = null })
            .SetName("Zheng_Roger_to_Unset"),
        new TestCaseData(
            CreateDepartmentRequest.Valid with { AdministratorName = null },
            Valid with { AdministratorName = null })
            .SetName("Unset_to_Unset")
    ];
}
