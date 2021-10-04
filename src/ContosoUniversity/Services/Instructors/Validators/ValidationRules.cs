namespace ContosoUniversity.Services.Instructors.Validators
{
    using FluentValidation;

    public static class ValidationRules
    {
        public static void SatisfiesLastNameRequirements<T>(this IRuleBuilder<T, string> rule) =>
            rule
                .MinimumLength(1)
                .MaximumLength(50)
                .Matches(@"^[A-Z]+[a-zA-Z''-'\s]*$")
                .WithMessage("The first character must upper case and the remaining characters must be alphabetical");
        
        public static void SatisfiesFirstNameRequirements<T>(this IRuleBuilder<T, string> rule) =>
            rule
                .NotNull()
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("First name cannot be longer than 50 characters.");

        public static void SatisfiesLocationRequirements<T>(this IRuleBuilder<T, string> rule) =>
            rule
                .MaximumLength(50);
    }
}