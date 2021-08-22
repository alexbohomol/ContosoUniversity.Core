namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
    using Domain.Instructor;

    using MediatR;

    public class EditInstructorCommand : IRequest
    {
        public Guid ExternalId { get; set; }

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

        public Guid[] SelectedCourses { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    }
    
    public class EditInstructorCommandHandler : AsyncRequestHandler<EditInstructorCommand>
    {
        private readonly IInstructorsRepository _instructorsRepository;

        public EditInstructorCommandHandler(IInstructorsRepository instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }
        
        protected override async Task Handle(EditInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructor = await _instructorsRepository.GetById(request.ExternalId, cancellationToken);
            if (instructor is null)
                throw new EntityNotFoundException(nameof(instructor), request.ExternalId);

            instructor.UpdatePersonalInfo(request.FirstName, request.LastName, request.HireDate);
            instructor.Courses = request.SelectedCourses;
            instructor.Office = request.HasAssignedOffice 
                ? new OfficeAssignment(request.Location) 
                : null;

            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}