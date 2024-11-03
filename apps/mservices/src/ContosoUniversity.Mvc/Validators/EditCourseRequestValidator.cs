namespace ContosoUniversity.Mvc.Validators;

using Application;

using Courses.Core;

using FluentValidation;

using ViewModels.Courses;

public class EditCourseRequestValidator : AbstractValidator<EditCourseRequest>
{
    public EditCourseRequestValidator()
    {
        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();
    }
}
