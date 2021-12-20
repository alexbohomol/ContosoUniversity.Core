namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Courses.Validators;

using FluentValidation;

using ViewModels.Courses;

public class CreateCourseFormValidator : AbstractValidator<CreateCourseForm>
{
    public CreateCourseFormValidator()
    {
        RuleFor(x => x.CourseCode).SatisfiesCourseCodeRequirements();
        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();
    }
}