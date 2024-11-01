using ICoursesRoRepository = Courses.Core.ICoursesRoRepository;
using IDepartmentsRoRepository = Departments.Core.IDepartmentsRoRepository;

namespace ContosoUniversity.Application.Courses.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using FluentValidation;

internal class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly ICoursesRoRepository _coursesRoRepository;

    public EditCourseCommandValidator(
        IDepartmentsRoRepository departmentsRepository,
        ICoursesRoRepository coursesRoRepository)
    {
        _departmentsRepository = departmentsRepository;
        _coursesRoRepository = coursesRoRepository;

        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();

        RuleFor(x => x.DepartmentId)
            .MustAsync(BeAnExistingDepartment)
            .WithMessage("Please select an existing department.");

        RuleFor(x => x.Id)
            .MustAsync(BeAnExistingCourse)
            .WithMessage("Please select an existing course.");
    }

    private async Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token) =>
        await _departmentsRepository.Exists(departmentId, token);

    private async Task<bool> BeAnExistingCourse(Guid id, CancellationToken token) =>
        await _coursesRoRepository.Exists(id, token);
}
