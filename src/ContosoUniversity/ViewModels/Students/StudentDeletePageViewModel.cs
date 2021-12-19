namespace ContosoUniversity.ViewModels.Students;

using System;
using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class StudentDeletePageViewModel
{
    public const string ErrMsgSaveChanges =
        "Delete failed. Try again, and if the problem persists see your system administrator.";

    public StudentDeletePageViewModel(Student student)
    {
        ArgumentNullException.ThrowIfNull(student, nameof(student));

        LastName = student.LastName;
        FirstMidName = student.FirstName;
        EnrollmentDate = student.EnrollmentDate;
        ExternalId = student.ExternalId;
    }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; }

    [Display(Name = "Last Name")] public string LastName { get; }

    [Display(Name = "First Name")] public string FirstMidName { get; }

    public Guid ExternalId { get; }
}