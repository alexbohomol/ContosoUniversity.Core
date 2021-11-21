namespace ContosoUniversity.Services.Students.Validators;

using Commands;

using FluentValidation;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x).SetInheritanceValidator(
            x => { x.Add(new CreateStudentFormValidator()); });
    }
}