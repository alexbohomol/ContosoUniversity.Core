namespace ContosoUniversity.Services.Departments.Validators;

using FluentValidation;

/// <summary>
///     https://github.com/FluentValidation/FluentValidation/issues/184#issuecomment-197952324
/// </summary>
public static class ValidationRules
{
    public static void SatisfiesNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule
            .Length(3, 50);
    }
}