namespace ContosoUniversity.Data.Students.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain;

public class Student : IIdentifiable<Guid>
{
    [DataType(DataType.Date)] public DateTime EnrollmentDate { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
    public int Id { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    [Column("FirstName")]
    public string FirstMidName { get; set; }

    [Display(Name = "Full Name")] public string FullName => LastName + ", " + FirstMidName;

    public Guid ExternalId { get; set; }
}