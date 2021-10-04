namespace ContosoUniversity.Services.Instructors.Validators
{
    using Commands;

    using FluentValidation;

    public class EditInstructorCommandValidator : AbstractValidator<EditInstructorCommand>
    {
        public EditInstructorCommandValidator()
        {
            RuleFor(x => x).SetInheritanceValidator(x =>
            {
                x.Add(new EditInstructorFormValidator());
            });

            RuleFor(x => x.ExternalId).NotEmpty();
        }
    }
}