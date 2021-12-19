namespace ContosoUniversity.Validators;

using FluentValidation;

using Services.Departments.Validators;

using ViewModels.Departments;

public class EditDepartmentFormValidator : AbstractValidator<EditDepartmentForm>
{
    public EditDepartmentFormValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}