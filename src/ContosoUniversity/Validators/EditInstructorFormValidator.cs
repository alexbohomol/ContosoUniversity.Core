namespace ContosoUniversity.Validators;

using Application.Services.Instructors.Validators;

using FluentValidation;

using ViewModels.Instructors;

public class EditInstructorFormValidator : AbstractValidator<EditInstructorForm>
{
    public EditInstructorFormValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();
    }
}