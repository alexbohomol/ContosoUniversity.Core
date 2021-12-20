namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Instructors.Validators;

using FluentValidation;

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