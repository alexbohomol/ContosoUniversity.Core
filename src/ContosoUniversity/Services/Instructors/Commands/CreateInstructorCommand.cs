namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;
    using Data.Departments.Models;

    using MediatR;

    public class CreateInstructorCommand : IRequest
    {
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "The first character must upper case and the remaining characters must be alphabetical")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public string[] SelectedCourses { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    }
    
    public class CreateInstructorCommandHandler : AsyncRequestHandler<CreateInstructorCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public CreateInstructorCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
        {
            var instructor = new Instructor
            {
                ExternalId = Guid.NewGuid(),
                FirstMidName = command.FirstName,
                LastName = command.LastName,
                HireDate = command.HireDate,
                OfficeAssignment = command.HasAssignedOffice
                    ? new OfficeAssignment {Location = command.Location}
                    : null
            };

            instructor.CourseAssignments = command.SelectedCourses?.Select(x => new CourseAssignment
            {
                InstructorId = instructor.Id, // not yet generated ???
                CourseExternalId = Guid.Parse(x)
            }).ToList();

            _departmentsContext.Add(instructor);
            
            await _departmentsContext.SaveChangesAsync();
        }
    }
}