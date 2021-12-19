namespace ContosoUniversity.Services.Courses.Validators;

using System;

using Domain.Course;

using FluentValidation;

/// <summary>
///     https://github.com/FluentValidation/FluentValidation/issues/184#issuecomment-197952324
/// </summary>
public static class ValidationRules
{
    private const string ErrMsgTitle =
        "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";

    private static string ErrMsgCredits =>
        $"The field 'Credits' must be between {Credits.MinValue} and {Credits.MaxValue}.";

    private static string ErrMsgCourseCode =>
        $"Course code can have a value from {CourseCode.MinValue} to {CourseCode.MaxValue}.";

    public static void SatisfiesCourseCodeRequirements<T>(this IRuleBuilder<T, int> rule)
    {
        rule
            .InclusiveBetween(CourseCode.MinValue, CourseCode.MaxValue)
            .WithMessage(ErrMsgCourseCode);
    }

    public static void SatisfiesTitleRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule
            .Length(Course.TitleMinLength, Course.TitleMaxLength)
            .WithMessage(ErrMsgTitle);
    }

    public static void SatisfiesCreditsRequirements<T>(this IRuleBuilder<T, int> rule)
    {
        rule
            .InclusiveBetween(Credits.MinValue, Credits.MaxValue)
            .WithMessage(ErrMsgCredits);
    }

    public static void Required<T>(this IRuleBuilder<T, Guid> rule)
    {
        rule
            .NotEmpty()
            .WithMessage("Please select a course department.");
    }
}