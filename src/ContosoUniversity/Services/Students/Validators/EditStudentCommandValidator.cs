namespace ContosoUniversity.Services.Students.Validators;

using Commands;

using FluentValidation;

public class EditStudentCommandValidator : AbstractValidator<EditStudentCommand>
{
    public EditStudentCommandValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
    }
}