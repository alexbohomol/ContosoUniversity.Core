namespace ContosoUniversity.Application.Services.Students.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using Contracts.Repositories.ReadOnly;

using Courses.Validators;

using FluentValidation;

internal class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
{
    private readonly IStudentsRoRepository _studentsRoRepository;

    public DeleteStudentCommandValidator(
        IStudentsRoRepository studentsRoRepository)
    {
        _studentsRoRepository = studentsRoRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(BeAnExistingStudent)
            .WithMessage("Please select an existing student.")
            .Required();
    }

    private async Task<bool> BeAnExistingStudent(Guid id, CancellationToken cancellationToken) =>
        await _studentsRoRepository.Exists(id, cancellationToken);
}
