namespace ContosoUniversity.Application.Services.Students.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Courses.Validators;

using FluentValidation;

using global::Departments.Core;

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
