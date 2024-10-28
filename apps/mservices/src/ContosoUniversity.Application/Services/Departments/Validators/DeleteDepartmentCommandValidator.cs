namespace ContosoUniversity.Application.Services.Departments.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using Contracts.Repositories.ReadOnly;

using Courses.Validators;

using FluentValidation;

internal class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    private readonly IDepartmentsRoRepository _departmentsRoRepository;

    public DeleteDepartmentCommandValidator(
        IDepartmentsRoRepository departmentsRoRepository)
    {
        _departmentsRoRepository = departmentsRoRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(BeAnExistingDepartment)
            .WithMessage("Please select an existing department.")
            .Required();

        // RuleFor(x => x.RowVersion).NotEmpty(); // TODO: we must have version on delete
    }

    private async Task<bool> BeAnExistingDepartment(Guid id, CancellationToken token) =>
        await _departmentsRoRepository.Exists(id, token);
}
