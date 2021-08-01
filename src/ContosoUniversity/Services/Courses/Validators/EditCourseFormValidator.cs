namespace ContosoUniversity.Services.Courses.Validators
{
    using Domain.Course;

    using FluentValidation;

    using ViewModels.Courses;

    public class EditCourseFormValidator : AbstractValidator<CourseEditForm>
    {
        private const string ErrMsgTitle = "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";

        public EditCourseFormValidator()
        {
            // POST model rules
            RuleFor(x => x.Title)
                .Length(3, 50)
                .WithMessage(ErrMsgTitle);
            RuleFor(x => x.Credits)
                .InclusiveBetween(Credits.MinValue, Credits.MaxValue)
                .WithMessage(ErrMsgCredits);
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage("Please select a course department.");
        }

        private static string ErrMsgCredits => $"The field '{nameof(CreateCourseForm.Credits)}' must be between {Credits.MinValue} and {Credits.MaxValue}.";
    }
}