namespace ContosoUniversity.Application.Services.Instructors.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

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
    public bool HasAssignedCourses =>
        SelectedCourses is not null
        && SelectedCourses.Length != 0;
}

internal class CreateInstructorCommandHandler(IInstructorsRwRepository instructorsRepository)
    : IRequestHandler<CreateInstructorCommand>
{
    public async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
    {
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
