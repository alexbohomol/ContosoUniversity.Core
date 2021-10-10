namespace ContosoUniversity.Services.Students.Validators
{
    using FluentValidation;

    public static class ValidationRules
    {
        private const int NameMaxLength = 50;
        private const string ErrMsgFirstNameExceedsLength = "First name cannot be longer than 50 characters.";

        public static void SatisfiesLastNameRequirements<T>(this IRuleBuilder<T, string> rule) => 
            rule
                .NotNull()
                .NotEmpty()
                .MaximumLength(NameMaxLength);

        public static void SatisfiesFirstNameRequirements<T>(this IRuleBuilder<T, string> rule) => 
            rule
                .NotNull()
                .NotEmpty()
                .MaximumLength(NameMaxLength)
                .WithMessage(ErrMsgFirstNameExceedsLength);
    }
}