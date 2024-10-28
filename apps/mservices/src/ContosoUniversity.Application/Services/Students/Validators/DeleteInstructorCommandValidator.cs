namespace ContosoUniversity.Application.Services.Students.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;

using Courses.Validators;

using FluentValidation;

using Instructors.Commands;

internal class DeleteInstructorCommandValidator : AbstractValidator<DeleteInstructorCommand>
{
    private readonly IInstructorsRoRepository _repository;

    public DeleteInstructorCommandValidator(
        IInstructorsRoRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(BeAnExistingInstructor)
            .WithMessage("Please select an existing instructor.")
            .Required();
    }

    private async Task<bool> BeAnExistingInstructor(Guid id, CancellationToken cancellationToken) =>
        await _repository.Exists(id, cancellationToken);
}
