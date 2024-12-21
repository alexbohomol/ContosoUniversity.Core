namespace ContosoUniversity.Mvc.Validators;

using Departments.Core.Handlers.Commands;

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
