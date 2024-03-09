namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Students.Validators;

using FluentValidation;

using ViewModels.Students;

internal class EditStudentRequestValidator : AbstractValidator<EditStudentRequest>
{
    public EditStudentRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}
