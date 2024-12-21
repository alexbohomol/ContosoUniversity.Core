namespace ContosoUniversity.Mvc.Validators;

using Departments.Core.Handlers.Commands;

using FluentValidation;

using ViewModels.Departments;

public class EditDepartmentRequestValidator : AbstractValidator<EditDepartmentRequest>
{
    public EditDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).SatisfiesNameRequirements();
    }
}
