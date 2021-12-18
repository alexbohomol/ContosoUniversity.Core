namespace ContosoUniversity.Services.Instructors.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;
using Application.Exceptions;

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

        instructor.AssignCourses(request.SelectedCourses);

        if (request.HasAssignedOffice)
            instructor.AssignOffice(new OfficeAssignment(request.Location));
        else
            instructor.ResetOffice();

        await _instructorsRepository.Save(instructor, cancellationToken);
    }
}