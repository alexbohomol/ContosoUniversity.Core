namespace ContosoUniversity.Validators;

using FluentValidation;

using Services.Instructors.Validators;

using ViewModels.Instructors;

public class CreateInstructorFormValidator : AbstractValidator<CreateInstructorForm>
{
    public CreateInstructorFormValidator()
    {
        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();
    }
}