namespace ContosoUniversity.Mvc.Validators;

using Application.Students.Validators;

using FluentValidation;

using ViewModels.Students;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}
