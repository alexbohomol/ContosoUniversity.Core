namespace ContosoUniversity.Application.Services.Instructors.Commands;

using System;
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
    public bool HasAssignedCourses =>
        SelectedCourses is not null
        && SelectedCourses.Length != 0;
}

internal class EditInstructorCommandHandler(IInstructorsRwRepository instructorsRepository) : IRequestHandler<EditInstructorCommand>
{
    public async Task Handle(EditInstructorCommand request, CancellationToken cancellationToken)
    {
        Instructor instructor = await instructorsRepository.GetById(request.ExternalId, cancellationToken);
        if (instructor is null)
        {
            throw new EntityNotFoundException(nameof(instructor), request.ExternalId);
        }

        instructor.UpdatePersonalInfo(request.FirstName, request.LastName, request.HireDate);

        if (request.HasAssignedCourses)
        {
            instructor.AssignCourses(request.SelectedCourses);
        }
        else
        {
            instructor.ResetCourseAssignments();
        }

        if (request.HasAssignedOffice)
        {
            instructor.AssignOffice(new OfficeAssignment(request.Location));
        }
        else
        {
            instructor.ResetOffice();
        }

        await instructorsRepository.Save(instructor, cancellationToken);
    }
}
