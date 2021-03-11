namespace ContosoUniversity.Services.Validators.Courses
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Data.Departments;

    using Domain;
    using Domain.Contracts;

    using FluentValidation;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Courses;

    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        private const string ErrMsgTitle = "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;

        public CreateCourseCommandValidator(
            ICoursesRepository coursesRepository,
            DepartmentsContext departmentsContext)
        {
            _coursesRepository = coursesRepository;
            _departmentsContext = departmentsContext;

            // POST model rules
            RuleFor(x => x.CourseCode)
                .InclusiveBetween(CourseCode.MinValue, CourseCode.MaxValue)
                .WithMessage(ErrMsgCourseCode);
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
            RuleFor(x => x.CourseCode)
                .MustAsync(BeANewCourseCode)
                .WithMessage("Please select a new course code.");
            RuleFor(x => x.DepartmentId)
                .MustAsync(BeAnExistingDepartment)
                .WithMessage("Please select an existing department.");
        }

        private string ErrMsgCourseCode => $"Course code can have a value from {CourseCode.MinValue} to {CourseCode.MaxValue}.";
        private static string ErrMsgCredits => $"The field '{nameof(CreateCourseForm.Credits)}' must be between {Credits.MinValue} and {Credits.MaxValue}.";

        private async Task<bool> BeANewCourseCode(int courseCode, CancellationToken token)
        {
            return !await _coursesRepository.ExistsCourseCode(courseCode);
        }

        private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token)
        {
            return _departmentsContext
                .Departments
                .AnyAsync(x => x.ExternalId == departmentId, token);
        }
    }
}