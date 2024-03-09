namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Student;

using Exceptions;

using MediatR;

public class EditStudentCommand : IRequest
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    public Guid ExternalId { get; set; }
}

internal class EditStudentCommandHandler(IStudentsRwRepository studentsRepository) : IRequestHandler<EditStudentCommand>
{
    public async Task Handle(EditStudentCommand request, CancellationToken cancellationToken)
    {
        Student student = await studentsRepository.GetById(request.ExternalId, cancellationToken);
        if (student == null)
        {
            throw new EntityNotFoundException(nameof(student), request.ExternalId);
        }

        student.UpdatePersonInfo(request.LastName, request.FirstName);
        student.Enroll(request.EnrollmentDate);

        await studentsRepository.Save(student, cancellationToken);
    }
}
