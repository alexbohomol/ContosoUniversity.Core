using Instructor = Departments.Core.Domain.Instructor;
using OfficeAssignment = Departments.Core.Domain.OfficeAssignment;

namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class EditInstructorCommandHandler(
    IInstructorsRwRepository instructorsRepository)
    : IRequestHandler<EditInstructorCommand>
{
    public async Task Handle(EditInstructorCommand request, CancellationToken cancellationToken)
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
    }
}
