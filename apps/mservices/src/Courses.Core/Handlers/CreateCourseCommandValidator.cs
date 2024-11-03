namespace Courses.Core.Handlers;

using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

internal class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    private readonly ICoursesRoRepository _coursesRoRepository;
    // private readonly IDepartmentsRoRepository _departmentsRepository;

    public CreateCourseCommandValidator(
        ICoursesRoRepository coursesRoRepository)
    // IDepartmentsRoRepository departmentsRepository)
    {
        _coursesRoRepository = coursesRoRepository;
        // _departmentsRepository = departmentsRepository;

        // RuleFor(x => x.CourseCode).SatisfiesCourseCodeRequirements();
        // RuleFor(x => x.Title).SatisfiesTitleRequirements();
        // RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();

        RuleFor(x => x.CourseCode)
            .MustAsync(BeANewCourseCode)
            .WithMessage("Please select a new course code.");
        // RuleFor(x => x.DepartmentId)
        //     .MustAsync(BeAnExistingDepartment)
        //     .WithMessage("Please select an existing department.");
    }

    private async Task<bool> BeANewCourseCode(int courseCode, CancellationToken token)
    {
        return !await _coursesRoRepository.ExistsCourseCode(courseCode, token);
    }

    // private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token)
    // {
    //     return _departmentsRepository.Exists(departmentId, token);
    // }
}
