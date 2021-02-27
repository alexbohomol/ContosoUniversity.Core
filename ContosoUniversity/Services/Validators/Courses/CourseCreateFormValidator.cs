namespace ContosoUniversity.Services.Validators.Courses
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Courses;
    using Data.Departments;

    using Domain;

    using FluentValidation;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Courses;

    public class CourseCreateFormValidator : AbstractValidator<CourseCreateForm>
    {
        private readonly CoursesContext _coursesContext;
        private readonly DepartmentsContext _departmentsContext;
        
        public string ErrMsgCourseCode => $"Course code can have a value from {CourseCode.MinValue} to {CourseCode.MaxValue}.";
        private static string ErrMsgCredits => $"The field '{nameof(CourseCreateForm.Credits)}' must be between {Credits.MinValue} and {Credits.MaxValue}.";
        private const string ErrMsgTitle = "The field '{PropertyName}' must be a string with a minimum length of {MinLength} and a maximum length of {MaxLength}.";

        public CourseCreateFormValidator(
            CoursesContext coursesContext,
            DepartmentsContext departmentsContext)
        {
            _coursesContext = coursesContext;
            _departmentsContext = departmentsContext;

            // POST model rules
            RuleFor(x => x.CourseCode).InclusiveBetween(CourseCode.MinValue, CourseCode.MaxValue).WithMessage(ErrMsgCourseCode);
            RuleFor(x => x.Title).Length(3, 50).WithMessage(ErrMsgTitle);
            RuleFor(x => x.Credits).InclusiveBetween(Credits.MinValue, Credits.MaxValue).WithMessage(ErrMsgCredits);
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Please select a course department.");
            
            // domain rules
            RuleFor(x => x.CourseCode).MustAsync(BeANewCourseCode).WithMessage("Please select a new course code.");
            RuleFor(x => x.DepartmentId).MustAsync(BeAnExistingDepartment).WithMessage("Please select an existing department.");
        }
        
        private Task<bool> BeANewCourseCode(int courseCode, CancellationToken token) => 
            _coursesContext
                .Courses
                .AllAsync(x => x.CourseCode != courseCode, token);

        private Task<bool> BeAnExistingDepartment(Guid departmentId, CancellationToken token) =>
            _departmentsContext
                .Departments
                .AnyAsync(x => x.ExternalId == departmentId, token);
    }
}