namespace ContosoUniversity.Benchmarking.Factories;

using System;

internal static class IdsFactory
{
    private static readonly Guid[] DefaultCourseIds =
    [
        new("51f60b7d-fb0c-40eb-a74b-b2d90157afa0"),
        new("7f4a2bf3-8623-4d4b-a555-7e1c18da1d31"),
        new("42153736-0a08-49ef-84a1-7718189945ca"),
        new("f3e9966c-467b-4b99-90ca-a29bae85ca94"),
        new("8ebb5543-371a-4c5b-a72b-09bc9f615e36"),
        new("d53ffc3d-aa4e-41cf-8f0e-435c73889dcf"),
        new("1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4")
    ];
    private static readonly int DefaultCourseIdsLength = DefaultCourseIds.Length;

    private static readonly Guid[] DefaultDepartmentIds =
    [
        new("31a130fe-b396-4bb8-88d3-26fa8778b4c6"),
        new("dab7e678-e3e7-4471-8282-96fe52e5c16f"),
        new("72c0804d-b208-4e67-82ba-cf54dc93dcc8"),
        new("377c186a-6782-4367-9246-e5fe4195a97c")
    ];
    private static readonly int DefaultDepartmentIdsLength = DefaultDepartmentIds.Length;

    public static Guid SelectCourseIdForIteration(int iteration)
        => DefaultCourseIds[iteration % DefaultCourseIdsLength];

    public static Guid SelectDepartmentIdForIteration(int iteration)
        => DefaultDepartmentIds[iteration % DefaultDepartmentIdsLength];
}
