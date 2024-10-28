namespace ContosoUniversity.Application.Services.Instructors.Validators;

using Domain.Instructor;

using FluentValidation;

public static class ValidationRules
{
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
