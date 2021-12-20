namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class InstructorListItemViewModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    public string FirstName { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }


    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime HireDate { get; set; }

    public string Office { get; set; }
    public IEnumerable<Guid> AssignedCourseIds { get; set; }
    public IEnumerable<string> AssignedCourses { get; set; }
    public string RowClass { get; set; }
}