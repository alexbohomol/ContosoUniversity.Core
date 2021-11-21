namespace ContosoUniversity.Services.Courses.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using Domain.Contracts;

using FluentValidation;

public class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;

    public EditCourseCommandValidator(IDepartmentsRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;

        RuleFor(x => x).SetInheritanceValidator(x => { x.Add(v => new EditCourseFormValidator()); });

        RuleFor(x => x.DepartmentId)
            .MustAsync(BeAnExistingDepartment)
            .WithMessage("Please select an existing department.");
    }

    private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token)
    {
        return _departmentsRepository.Exists(departmentId, token);
    }
}