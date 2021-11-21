namespace ContosoUniversity.Services.Courses.Validators;

using System;

using Domain.Course;

using FluentValidation;

using ViewModels.Courses;

/// <summary>
///     https://github.com/FluentValidation/FluentValidation/issues/184#issuecomment-197952324
/// </summary>
public static class ValidationRules
{
    private const string ErrMsgTitle =
        "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";

    private static string ErrMsgCredits =>
        $"The field '{nameof(CreateCourseForm.Credits)}' must be between {Credits.MinValue} and {Credits.MaxValue}.";

    public static void SatisfiesTitleRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule
            .Length(3, 50)
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