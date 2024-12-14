using Student = Students.Core.Domain.Student;

namespace Students.Core.Handlers.Commands;

using System;

using FluentValidation;

public static class ValidationRules
{
    public static void SatisfiesLastNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotNull()
            .NotEmpty()
            .MaximumLength(Student.LastNameMaxLength)
            .WithMessage($"Last name cannot be longer than {Student.LastNameMaxLength} characters.");
        //TODO: default message creates locale issues between CI/local tests runs
    }

    public static void SatisfiesFirstNameRequirements<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotNull()
            .NotEmpty()
            .MaximumLength(Student.FirstNameMaxLength)
            .WithMessage($"First name cannot be longer than {Student.FirstNameMaxLength} characters.");
        //TODO: default message creates locale issues between CI/local tests runs
    }

    public static void Required<T>(this IRuleBuilder<T, Guid> rule)
    {
        rule
            .NotEmpty()
            .WithMessage("Please select an existing entity.");
    }
}
