namespace ContosoUniversity.Domain.Course;

using System;

/// <summary>
///     https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators
/// </summary>
public readonly struct CourseCode
{
    public const int MinValue = 1000;
    public const int MaxValue = 9999;

    private readonly int _code;

    private CourseCode(int code)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(code, MinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(code, MaxValue);

        _code = code;
    }

    public static implicit operator CourseCode(int code) => ToCourseCode(code);

    public static implicit operator int(CourseCode code) => ToInt32(code);

    public static CourseCode ToCourseCode(int code) => new(code);

    public static int ToInt32(CourseCode code) => code._code;
}
