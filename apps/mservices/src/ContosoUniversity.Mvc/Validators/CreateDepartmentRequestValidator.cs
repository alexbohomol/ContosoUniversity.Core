namespace ContosoUniversity.Mvc.Validators;

using Departments.Core.Handlers.Commands;

using FluentValidation;

using ViewModels.Departments;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
