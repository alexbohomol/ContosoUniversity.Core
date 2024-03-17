namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Departments.Validators;

using FluentValidation;

using ViewModels.Departments;

internal class EditDepartmentRequestValidator : AbstractValidator<EditDepartmentRequest>
{
    public EditDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
