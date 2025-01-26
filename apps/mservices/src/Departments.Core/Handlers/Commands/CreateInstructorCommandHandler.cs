namespace Departments.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class CreateInstructorCommandHandler(
    IInstructorsRwRepository instructorsRepository)
    : IRequestHandler<CreateInstructorCommand, Instructor>
{
    public async Task<Instructor> Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var instructor = Instructor.Create(
            command.FirstName,
            command.LastName,
            command.HireDate);

        if (command.HasAssignedCourses)
        {
            instructor.AssignCourses(command.SelectedCourses);
        }

        if (command.HasAssignedOffice)
        {
            instructor.AssignOffice(new OfficeAssignment(command.Location));
        }

        await instructorsRepository.Save(instructor, cancellationToken);

        return instructor;
    }
}
