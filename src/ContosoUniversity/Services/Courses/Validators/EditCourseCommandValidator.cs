namespace ContosoUniversity.Services.Courses.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;

using Commands;

using FluentValidation;

public class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;

    public EditCourseCommandValidator(IDepartmentsRoRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;

        RuleFor(x => x.Title).SatisfiesTitleRequirements();
        RuleFor(x => x.Credits).SatisfiesCreditsRequirements();
        RuleFor(x => x.DepartmentId).Required();

        RuleFor(x => x.DepartmentId)
            .MustAsync(BeAnExistingDepartment)
            .WithMessage("Please select an existing department.");
    }

    private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token)
    {
        return _departmentsRepository.Exists(departmentId, token);
    }
}