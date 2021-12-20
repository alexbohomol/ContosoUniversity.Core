namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Courses.Validators;

using FluentValidation;

using ViewModels.Courses;

public class EditCourseFormValidator : AbstractValidator<CourseEditForm>
{
    public EditCourseFormValidator()
    {
        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();
    }
}