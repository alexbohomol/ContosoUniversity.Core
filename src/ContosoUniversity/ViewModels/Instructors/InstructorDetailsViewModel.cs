namespace ContosoUniversity.ViewModels.Instructors;

using System;
using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class InstructorDetailsViewModel
{
    public InstructorDetailsViewModel(Instructor instructor)
    {
        ArgumentNullException.ThrowIfNull(instructor, nameof(instructor));

        LastName = instructor.LastName;
        FirstName = instructor.FirstName;
        HireDate = instructor.HireDate;
        ExternalId = instructor.ExternalId;
    }

    [DataType(DataType.Date)]
    [Display(Name = "Hire Date")]
    public DateTime HireDate { get; }

    [Display(Name = "Last Name")] public string LastName { get; }

    [Display(Name = "First Name")] public string FirstName { get; }

    public Guid ExternalId { get; }
}