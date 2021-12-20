namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Students.Validators;

using FluentValidation;

using ViewModels.Students;

public class EditStudentFormValidator : AbstractValidator<EditStudentForm>
{
    public EditStudentFormValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}