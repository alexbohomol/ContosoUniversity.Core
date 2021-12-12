namespace ContosoUniversity.Services.Courses.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using Domain.Contracts;

using FluentValidation;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    private readonly ICoursesRoRepository _coursesRepository;
    private readonly IDepartmentsRoRepository _departmentsRepository;

    public CreateCourseCommandValidator(
        ICoursesRoRepository coursesRepository,
        IDepartmentsRoRepository departmentsRepository)
    {
        _coursesRepository = coursesRepository;
        _departmentsRepository = departmentsRepository;

        RuleFor(x => x).SetInheritanceValidator(x => { x.Add(v => new CreateCourseFormValidator()); });

        RuleFor(x => x.CourseCode)
            .MustAsync(BeANewCourseCode)
            .WithMessage("Please select a new course code.");
        RuleFor(x => x.DepartmentId)
            .MustAsync(BeAnExistingDepartment)
            .WithMessage("Please select an existing department.");
    }

    private async Task<bool> BeANewCourseCode(int courseCode, CancellationToken token)
    {
        return !await _coursesRepository.ExistsCourseCode(courseCode, token);
    }

    private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token)
    {
        return _departmentsRepository.Exists(departmentId, token);
    }
}