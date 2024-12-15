namespace ContosoUniversity.Mvc.Validators;

using FluentValidation;

using Students.Core.Handlers.Commands;

using ViewModels.Students;

public class EditStudentRequestValidator : AbstractValidator<EditStudentRequest>
{
    public EditStudentRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}
