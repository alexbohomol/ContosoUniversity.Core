namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;
using System.ComponentModel.DataAnnotations;

public class EditStudentRequest
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }

    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    public Guid ExternalId { get; set; }
}
