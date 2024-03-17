namespace ContosoUniversity.Mvc.Validators;

using Application.Services.Courses.Validators;

using FluentValidation;

using ViewModels.Courses;

internal class EditCourseRequestValidator : AbstractValidator<EditCourseRequest>
{
    public EditCourseRequestValidator()
    {
        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();
    }
}
