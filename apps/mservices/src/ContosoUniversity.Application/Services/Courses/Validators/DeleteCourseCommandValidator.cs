namespace ContosoUniversity.Application.Services.Courses.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

using Commands;

using FluentValidation;

using global::Courses.Core;

internal class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    private readonly ICoursesRoRepository _coursesRoRepository;

    public DeleteCourseCommandValidator(
        ICoursesRoRepository coursesRoRepository)
    {
        _coursesRoRepository = coursesRoRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(BeAnExistingCourse)
            .WithMessage("Please select an existing course.")
            .Required();
    }

    private async Task<bool> BeAnExistingCourse(Guid id, CancellationToken token) =>
        await _coursesRoRepository.Exists(id, token);
}
