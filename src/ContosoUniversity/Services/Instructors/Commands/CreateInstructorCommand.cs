namespace ContosoUniversity.Services.Instructors.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories;

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
    private readonly IInstructorsRwRepository _instructorsRepository;

    public CreateInstructorCommandHandler(IInstructorsRwRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    protected override async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
    {
        var instructor = Instructor.Create(
            command.FirstName,
            command.LastName,
            command.HireDate);

        instructor.AssignCourses(command.SelectedCourses);

        if (command.HasAssignedOffice)
            instructor.AssignOffice(new OfficeAssignment(command.Location));
        else
            instructor.ResetOffice();

        await _instructorsRepository.Save(instructor, cancellationToken);
    }
}