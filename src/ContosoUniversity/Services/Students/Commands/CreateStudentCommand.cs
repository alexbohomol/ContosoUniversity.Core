namespace ContosoUniversity.Services.Students.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Student;

    using MediatR;

    public class CreateStudentCommand : IRequest
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
    }
    
    public class CreateStudentCommandHandler : AsyncRequestHandler<CreateStudentCommand>
    {
        private readonly IStudentsRepository _repository;

        public CreateStudentCommandHandler(IStudentsRepository repository)
        {
            _repository = repository;
        }

        protected override Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            return _repository.Save(
                new Student(
                    request.LastName,
                    request.FirstName,
                    request.EnrollmentDate,
                    EnrollmentsCollection.Empty,
                    Guid.NewGuid()),
                cancellationToken);
        }
    }
}