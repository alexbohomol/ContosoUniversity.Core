namespace ContosoUniversity.Application;

using System;

using FluentValidation;

/// <summary>
///     https://github.com/FluentValidation/FluentValidation/issues/184#issuecomment-197952324
/// </summary>
public static class ValidationRules
{
    public static void Required<T>(this IRuleBuilder<T, Guid> rule)
    {
        rule
            .NotEmpty()
            .WithMessage("Please select an existing entity.");
    }
}
