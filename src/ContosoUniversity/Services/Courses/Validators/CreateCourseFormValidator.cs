namespace ContosoUniversity.Services.Courses.Validators
{
    using Domain.Course;

    using FluentValidation;

    using ViewModels.Courses;

    public class CreateCourseFormValidator : AbstractValidator<CreateCourseForm>
    {
        private const string ErrMsgTitle = "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";

        public CreateCourseFormValidator()
        {
            RuleFor(x => x.CourseCode)
                .InclusiveBetween(CourseCode.MinValue, CourseCode.MaxValue)
                .WithMessage(ErrMsgCourseCode);
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

        private string ErrMsgCourseCode => $"Course code can have a value from {CourseCode.MinValue} to {CourseCode.MaxValue}.";
        private static string ErrMsgCredits => $"The field '{nameof(CreateCourseForm.Credits)}' must be between {Credits.MinValue} and {Credits.MaxValue}.";
    }
}