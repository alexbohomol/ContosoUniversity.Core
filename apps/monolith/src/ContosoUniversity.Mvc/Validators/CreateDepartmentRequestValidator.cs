namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Departments.Validators;

using FluentValidation;

using ViewModels.Departments;

internal class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
