namespace ContosoUniversity.Mvc.Validators;

using Application.Courses.Validators;

using FluentValidation;

using ViewModels.Courses;

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.CourseCode).SatisfiesCourseCodeRequirements();
        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();
    }
}
