namespace ContosoUniversity.Mvc.Validators;

using Departments.Core.Handlers.Commands;

using FluentValidation;

using ViewModels.Instructors;

public class CreateInstructorRequestValidator : AbstractValidator<CreateInstructorRequest>
{
    public CreateInstructorRequestValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();
    }
}
