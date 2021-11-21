namespace ContosoUniversity.Services.Departments.Validators;

using FluentValidation;

using ViewModels.Departments;

public class CreateDepartmentFormValidator : AbstractValidator<CreateDepartmentForm>
{
    public CreateDepartmentFormValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}