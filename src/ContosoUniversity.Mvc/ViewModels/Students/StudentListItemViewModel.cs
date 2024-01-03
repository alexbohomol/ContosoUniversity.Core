namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;
using System.ComponentModel.DataAnnotations;

public class StudentListItemViewModel
{
    [Display(Name = "Last Name")] public string LastName { get; init; }

    [Display(Name = "First Name")] public string FirstName { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; init; }

    public Guid ExternalId { get; init; }
}
