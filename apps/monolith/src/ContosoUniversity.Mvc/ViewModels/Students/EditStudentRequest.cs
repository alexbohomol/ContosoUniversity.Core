namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;
using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;

public record EditStudentRequest
{
    public EditStudentRequest(Student student)
    {
        ArgumentNullException.ThrowIfNull(student, nameof(student));

        LastName = student.LastName;
        FirstName = student.FirstName;
        EnrollmentDate = student.EnrollmentDate;
        ExternalId = student.ExternalId;
    }

    public EditStudentRequest() { }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; init; }

    [Display(Name = "Last Name")]
    public string LastName { get; init; }

    [Display(Name = "First Name")]
    public string FirstName { get; init; }

    public Guid ExternalId { get; init; }
}
