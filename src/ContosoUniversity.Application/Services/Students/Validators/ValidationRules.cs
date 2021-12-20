namespace ContosoUniversity.Application.Services.Students.Validators;

using Domain.Student;

using FluentValidation;

public static class ValidationRules
{
    private static string ErrMsgFirstNameExceedsLength =>
        $"First name cannot be longer than {Student.FirstNameMaxLength} characters.";

    public static void SatisfiesLastNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotNull()
            .NotEmpty()
            .MaximumLength(Student.LastNameMaxLength);
    }

    public static void SatisfiesFirstNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotNull()
            .NotEmpty()
            .MaximumLength(Student.FirstNameMaxLength)
            .WithMessage(ErrMsgFirstNameExceedsLength);
    }
}