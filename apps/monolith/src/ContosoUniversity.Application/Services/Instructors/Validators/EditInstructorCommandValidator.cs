namespace ContosoUniversity.Application.Services.Instructors.Validators;

using Commands;

using FluentValidation;

public class EditInstructorCommandValidator : AbstractValidator<EditInstructorCommand>
{
    public EditInstructorCommandValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();

        RuleFor(x => x.ExternalId).NotEmpty();
    }
}
