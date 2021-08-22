namespace ContosoUniversity.Services.Students.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    public class EditStudentCommand : IRequest
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public Guid ExternalId { get; set; }
    }
    
    public class EditStudentCommandHandler : AsyncRequestHandler<EditStudentCommand>
    {
        private readonly IStudentsRepository _studentsRepository;

        public EditStudentCommandHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        protected override async Task Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.ExternalId);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.ExternalId);

            student.UpdatePersonInfo(request.LastName, request.FirstName);
            student.Enroll(request.EnrollmentDate);

            await _studentsRepository.Save(student);
        }
    }
}