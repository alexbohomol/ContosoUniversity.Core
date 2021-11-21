namespace ContosoUniversity.Services.Courses.Validators;

using Domain.Course;

using FluentValidation;

using ViewModels.Courses;

public class CreateCourseFormValidator : AbstractValidator<CreateCourseForm>
{
    public CreateCourseFormValidator()
    {
        RuleFor(x => x.CourseCode)
            .InclusiveBetween(CourseCode.MinValue, CourseCode.MaxValue)
            .WithMessage(ErrMsgCourseCode);
        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();
    }

    private string ErrMsgCourseCode =>
        $"Course code can have a value from {CourseCode.MinValue} to {CourseCode.MaxValue}.";
}