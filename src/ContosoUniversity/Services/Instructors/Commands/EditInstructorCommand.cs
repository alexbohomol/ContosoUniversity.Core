namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;
    using Domain.Instructor;

    using MediatR;

    public class EditInstructorCommand : IRequest
    {
        public Guid ExternalId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public Guid[] SelectedCourses { get; set; }

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