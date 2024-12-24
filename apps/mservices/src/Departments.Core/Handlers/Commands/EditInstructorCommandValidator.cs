namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

public class EditInstructorCommandValidator : AbstractValidator<EditInstructorCommand>
{
    private readonly IInstructorsRoRepository _repository;

    public EditInstructorCommandValidator(
        IInstructorsRoRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();
        RuleFor(x => x.Location).SatisfiesLocationRequirements();

        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .MustAsync(BeAnExistingInstructor)
            .WithMessage("Please select an existing instructor.")
            .Required();

        //TODO: validate hire date

        //TODO: validate courses must exist
    }

    private async Task<bool> BeAnExistingInstructor(Guid externalId, CancellationToken token) =>
        await _repository.Exists(externalId, token);
}
