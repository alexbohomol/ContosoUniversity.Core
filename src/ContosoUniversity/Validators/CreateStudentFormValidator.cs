namespace ContosoUniversity.Validators;

using Application.Services.Students.Validators;

using FluentValidation;

using ViewModels.Students;

public class CreateStudentFormValidator : AbstractValidator<CreateStudentForm>
{
    public CreateStudentFormValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}