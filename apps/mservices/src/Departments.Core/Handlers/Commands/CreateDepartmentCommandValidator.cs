namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    private readonly IInstructorsRoRepository _repository;

    public CreateDepartmentCommandValidator(
        IInstructorsRoRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name).SatisfiesNameRequirements();

        When(x => x.AdministratorId.HasValue, () =>
        {
            RuleFor(x => x.AdministratorId)
                .MustAsync((guid, token) => BeAnExistingInstructor(guid.Value, token))
                .WithMessage("Please select an existing instructor.");
        });
    }

    private async Task<bool> BeAnExistingInstructor(Guid administratorId, CancellationToken token) =>
        await _repository.Exists(administratorId, token);
}
