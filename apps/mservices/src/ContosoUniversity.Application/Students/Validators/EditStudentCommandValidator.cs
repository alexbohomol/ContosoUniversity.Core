using IStudentsRoRepository = Students.Core.IStudentsRoRepository;

namespace ContosoUniversity.Application.Students.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using Messaging.Contracts.Commands;

internal class EditStudentCommandValidator : AbstractValidator<EditStudentCommand>
{
    private readonly IStudentsRoRepository _studentsRoRepository;

    public EditStudentCommandValidator(
        IStudentsRoRepository studentsRoRepository)
    {
        _studentsRoRepository = studentsRoRepository;

        RuleFor(x => x.LastName).SatisfiesLastNameRequirements();
        RuleFor(x => x.FirstName).SatisfiesFirstNameRequirements();

        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .MustAsync(BeAnExistingStudent)
            .WithMessage("Please select an existing student.")
            .Required();
    }

    private async Task<bool> BeAnExistingStudent(Guid externalId, CancellationToken cancellationToken) =>
        await _studentsRoRepository.Exists(externalId, cancellationToken);
}
