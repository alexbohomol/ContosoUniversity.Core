namespace ContosoUniversity.Services.Students.Validators;

using Commands;

using FluentValidation;

public class EditStudentCommandValidator : AbstractValidator<EditStudentCommand>
{
    public EditStudentCommandValidator()
    {
        RuleFor(x => x).SetInheritanceValidator(
            x => { x.Add(new EditStudentFormValidator()); });
    }
}