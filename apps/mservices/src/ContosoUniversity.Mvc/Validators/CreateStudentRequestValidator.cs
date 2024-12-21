namespace ContosoUniversity.Mvc.Validators;

using FluentValidation;

using Students.Core.Handlers.Commands;

using ViewModels.Students;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}
