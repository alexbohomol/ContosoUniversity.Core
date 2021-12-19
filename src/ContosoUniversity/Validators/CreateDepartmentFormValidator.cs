namespace ContosoUniversity.Validators;

using FluentValidation;

using Services.Departments.Validators;

using ViewModels.Departments;

public class CreateDepartmentFormValidator : AbstractValidator<CreateDepartmentForm>
{
    public CreateDepartmentFormValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}