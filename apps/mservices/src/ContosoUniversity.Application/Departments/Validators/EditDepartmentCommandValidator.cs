using IDepartmentsRoRepository = Departments.Core.IDepartmentsRoRepository;
using IInstructorsRoRepository = Departments.Core.IInstructorsRoRepository;

namespace ContosoUniversity.Application.Departments.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Courses.Validators;

using FluentValidation;

using Messaging.Contracts.Commands;

internal class EditDepartmentCommandValidator : AbstractValidator<EditDepartmentCommand>
{
    private readonly IDepartmentsRoRepository _departmentsRoRepository;
    private readonly IInstructorsRoRepository _instructorsRoRepository;

    public EditDepartmentCommandValidator(
        IDepartmentsRoRepository departmentsRoRepository,
        IInstructorsRoRepository instructorsRoRepository)
    {
        _departmentsRoRepository = departmentsRoRepository;
        _instructorsRoRepository = instructorsRoRepository;

        RuleFor(x => x.Name).SatisfiesNameRequirements();

        RuleFor(x => x.Budget).GreaterThan(0);

        RuleFor(x => x.StartDate).NotEmpty();

        When(x => x.AdministratorId.HasValue, () =>
        {
            RuleFor(x => x.AdministratorId)
                .MustAsync((guid, token) => BeAnExistingInstructor(guid!.Value, token))
                .WithMessage("Please select an existing instructor.");
        });

        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .MustAsync(BeAnExistingDepartment)
            .WithMessage("Please select an existing department.")
            .Required();

        RuleFor(x => x.RowVersion).NotEmpty(); // TODO: extend to version check
    }

    private async Task<bool> BeAnExistingDepartment(Guid externalId, CancellationToken token) =>
        await _departmentsRoRepository.Exists(externalId, token);

    private async Task<bool> BeAnExistingInstructor(Guid administratorId, CancellationToken token) =>
        await _instructorsRoRepository.Exists(administratorId, token);
}
