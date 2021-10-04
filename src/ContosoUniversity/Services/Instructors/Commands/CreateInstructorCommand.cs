namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Instructor;

    using MediatR;

    public class CreateInstructorCommand : IRequest
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public Guid[] SelectedCourses { get; set; }
        public string Location { get; set; }

        public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    }
    
    public class CreateInstructorCommandHandler : AsyncRequestHandler<CreateInstructorCommand>
    {
        private readonly IInstructorsRepository _instructorsRepository;

        public CreateInstructorCommandHandler(IInstructorsRepository instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }
        
        protected override async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
        {
            var instructor = new Instructor(
                command.FirstName,
                command.LastName,
                command.HireDate,
                command.SelectedCourses,
                command.HasAssignedOffice
                    ? new OfficeAssignment(command.Location)
                    : null);

            await _instructorsRepository.Save(instructor, cancellationToken);
        }
    }
}