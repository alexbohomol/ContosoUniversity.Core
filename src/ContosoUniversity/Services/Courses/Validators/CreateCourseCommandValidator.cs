namespace ContosoUniversity.Services.Courses.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;
    using Domain.Course;

    using FluentValidation;

    using ViewModels.Courses;

    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        private const string ErrMsgTitle = "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public CreateCourseCommandValidator(
            ICoursesRepository coursesRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;

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

        private async Task<bool> BeANewCourseCode(int courseCode, CancellationToken token) => 
            !await _coursesRepository.ExistsCourseCode(courseCode);

        private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token) => 
            _departmentsRepository.Exists(departmentId);
    }
}