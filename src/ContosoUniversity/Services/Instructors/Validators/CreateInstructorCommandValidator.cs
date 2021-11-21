namespace ContosoUniversity.Services.Instructors.Validators;

using Commands;

using FluentValidation;

public class CreateInstructorCommandValidator : AbstractValidator<CreateInstructorCommand>
{
    public CreateInstructorCommandValidator()
    {
        RuleFor(x => x).SetInheritanceValidator(x => { x.Add(new CreateInstructorFormValidator()); });
    }
}