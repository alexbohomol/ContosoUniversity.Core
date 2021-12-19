namespace ContosoUniversity.Validators;

using FluentValidation;

using Services.Students.Validators;

using ViewModels.Students;

public class EditStudentFormValidator : AbstractValidator<EditStudentForm>
{
    public EditStudentFormValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}