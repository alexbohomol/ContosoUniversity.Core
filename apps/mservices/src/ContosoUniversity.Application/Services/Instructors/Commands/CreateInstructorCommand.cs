namespace ContosoUniversity.Application.Services.Instructors.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Departments.Core;
using global::Departments.Core.Domain;

using MediatR;

public record CreateInstructorCommand(
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location) : IRequest
{
    public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    public bool HasAssignedCourses =>
        SelectedCourses is not null
        && SelectedCourses.Length > 0;
}

internal class CreateInstructorCommandHandler(
    IInstructorsRwRepository instructorsRepository)
    : IRequestHandler<CreateInstructorCommand>
{
    public async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
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
    }
}
