namespace ContosoUniversity.Application.Services.Instructors.Commands;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Instructor;

using Exceptions;

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
    public bool HasAssignedCourses => SelectedCourses is not null && SelectedCourses.Any();
}

public class EditInstructorCommandHandler : AsyncRequestHandler<EditInstructorCommand>
{
    private readonly IInstructorsRwRepository _instructorsRepository;

    public EditInstructorCommandHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    protected override async Task Handle(EditInstructorCommand request, CancellationToken cancellationToken)
    {
        Instructor instructor = await _instructorsRepository.GetById(request.ExternalId, cancellationToken);
        if (instructor is null)
            throw new EntityNotFoundException(nameof(instructor), request.ExternalId);

        instructor.UpdatePersonalInfo(request.FirstName, request.LastName, request.HireDate);

        if (request.HasAssignedCourses)
            instructor.AssignCourses(request.SelectedCourses);
        else
            instructor.ResetCourseAssignments();

        if (request.HasAssignedOffice)
            instructor.AssignOffice(new OfficeAssignment(request.Location));
        else
            instructor.ResetOffice();

        await _instructorsRepository.Save(instructor, cancellationToken);
    }
}
