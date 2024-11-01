namespace ContosoUniversity.Mvc.Validators;

using Application.Students.Validators;

using FluentValidation;

using ViewModels.Students;

public class EditStudentRequestValidator : AbstractValidator<EditStudentRequest>
{
    public EditStudentRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}
