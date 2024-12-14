namespace Departments.Core.Handlers.Commands;

using System;

using Domain;

using FluentValidation;

/// <summary>
///     https://github.com/FluentValidation/FluentValidation/issues/184#issuecomment-197952324
/// </summary>
public static class ValidationRules
{
    // Departments

    public static void SatisfiesNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.Length(Department.NameMinLength, Department.NameMaxLength);
    }

    public static void Required<T>(this IRuleBuilder<T, Guid> rule)
    {
        rule
            .NotEmpty()
            .WithMessage("Please select an existing entity.");
    }

    // Instructors

    public static void SatisfiesLastNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.MinimumLength(Instructor.LastNameMinLength)
            .MaximumLength(Instructor.LastNameMaxLength)
            .Matches(@"^[A-Z]+[a-zA-Z''-'\s]*$")
            .WithMessage("The first character must upper case and the remaining characters must be alphabetical");
    }

    public static void SatisfiesFirstNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotNull()
            .NotEmpty()
            .MaximumLength(Instructor.FirstNameMaxLength)
            .WithMessage($"First name cannot be longer than {Instructor.FirstNameMaxLength} characters.");
    }

    public static void SatisfiesLocationRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.MaximumLength(OfficeAssignment.TitleMaxLength);
    }
}
