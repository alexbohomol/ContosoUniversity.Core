namespace ContosoUniversity.Mvc.Validators;

using Application.Departments.Validators;

using FluentValidation;

using ViewModels.Departments;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
