namespace ContosoUniversity.Mvc.Validators;

using Application.Instructors.Validators;

using FluentValidation;

using ViewModels.Instructors;

public class EditInstructorRequestValidator : AbstractValidator<EditInstructorRequest>
{
    public EditInstructorRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();
    }
}
