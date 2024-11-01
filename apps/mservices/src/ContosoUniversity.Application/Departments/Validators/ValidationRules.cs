using Department = Departments.Core.Domain.Department;

namespace ContosoUniversity.Application.Departments.Validators;

using FluentValidation;

/// <summary>
///     https://github.com/FluentValidation/FluentValidation/issues/184#issuecomment-197952324
/// </summary>
public static class ValidationRules
{
    public static void SatisfiesNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.Length(Department.NameMinLength, Department.NameMaxLength);
    }
}
