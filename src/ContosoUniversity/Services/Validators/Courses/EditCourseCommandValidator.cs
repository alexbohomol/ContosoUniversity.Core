namespace ContosoUniversity.Services.Validators.Courses
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Domain.Contracts;
    using Domain.Course;

    using FluentValidation;

    using ViewModels.Courses;

    public class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
    {
        private readonly IDepartmentsRepository _departmentsRepository;
        private const string ErrMsgTitle = "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";

        public EditCourseCommandValidator(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;

            // POST model rules
            RuleFor(x => x.Title)
                .Length(3, 50)
                .WithMessage(ErrMsgTitle);
            RuleFor(x => x.Credits)
                .InclusiveBetween(Credits.MinValue, Credits.MaxValue)
                .WithMessage(ErrMsgCredits);
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage("Please select a course department.");

            // domain rules
            RuleFor(x => x.DepartmentId)
                .MustAsync(BeAnExistingDepartment)
                .WithMessage("Please select an existing department.");
        }

        private static string ErrMsgCredits => $"The field '{nameof(CreateCourseForm.Credits)}' must be between {Credits.MinValue} and {Credits.MaxValue}.";

        private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token) => 
            _departmentsRepository.Exists(departmentId);
    }
}