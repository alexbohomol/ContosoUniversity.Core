namespace ContosoUniversity.Validators;

using FluentValidation;

using Services.Students.Validators;

using ViewModels.Students;

public class CreateStudentFormValidator : AbstractValidator<CreateStudentForm>
{
    public CreateStudentFormValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}