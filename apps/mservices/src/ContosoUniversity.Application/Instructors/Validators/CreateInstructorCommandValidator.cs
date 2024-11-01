namespace ContosoUniversity.Application.Instructors.Validators;

using Commands;

using FluentValidation;

public class CreateInstructorCommandValidator : AbstractValidator<CreateInstructorCommand>
{
    public CreateInstructorCommandValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();

        //TODO: validate hire date

        //TODO: validate courses must exist
    }
}
