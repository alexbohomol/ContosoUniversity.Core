namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class EditInstructorCommandHandler(
    IInstructorsRwRepository instructorsRepository)
    : IRequestHandler<EditInstructorCommand, Instructor>
{
    public async Task<Instructor> Handle(EditInstructorCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Instructor instructor = await instructorsRepository.GetById(request.ExternalId, cancellationToken);

        ArgumentNullException.ThrowIfNull(instructor);

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

        return instructor;
    }
}
