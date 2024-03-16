namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Departments.Validators;

using FluentValidation;

using ViewModels.Departments;

public class EditDepartmentFormValidator : AbstractValidator<EditDepartmentForm>
{
    public EditDepartmentFormValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
