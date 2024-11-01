namespace ContosoUniversity.Mvc.Validators;

using Application.Departments.Validators;

using FluentValidation;

using ViewModels.Departments;

public class EditDepartmentRequestValidator : AbstractValidator<EditDepartmentRequest>
{
    public EditDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
