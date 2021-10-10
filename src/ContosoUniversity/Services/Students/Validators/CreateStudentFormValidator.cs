namespace ContosoUniversity.Services.Students.Validators
{
    using FluentValidation;

    using ViewModels.Students;

    public class CreateStudentFormValidator : AbstractValidator<CreateStudentForm>
    {
        public CreateStudentFormValidator()
        {
            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);
            
            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("First name cannot be longer than 50 characters.");
        }
    }
}