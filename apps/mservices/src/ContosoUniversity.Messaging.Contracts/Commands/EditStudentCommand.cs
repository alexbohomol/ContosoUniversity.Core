namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;
using System.ComponentModel.DataAnnotations;

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
