namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Student;

using MediatR;

public record EditStudentCommand : IRequest
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; init; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; init; }

    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; init; }

    public Guid ExternalId { get; init; }
}

internal class EditStudentCommandHandler(
    IStudentsRwRepository studentsRepository)
    : IRequestHandler<EditStudentCommand>
{
    public async Task Handle(
        EditStudentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Student student = await studentsRepository.GetById(request.ExternalId, cancellationToken);

        ArgumentNullException.ThrowIfNull(student);

        student.UpdatePersonInfo(request.LastName, request.FirstName);
        student.Enroll(request.EnrollmentDate);

        await studentsRepository.Save(student, cancellationToken);
    }
}
