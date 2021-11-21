namespace ContosoUniversity.ViewModels.Instructors;

using System;
using System.ComponentModel.DataAnnotations;

public class InstructorDetailsViewModel
{
    [DataType(DataType.Date)]
    [Display(Name = "Hire Date")]
    public DateTime HireDate { get; set; }

    [Display(Name = "Last Name")] public string LastName { get; set; }

    [Display(Name = "First Name")] public string FirstName { get; set; }

    public Guid ExternalId { get; set; }
}