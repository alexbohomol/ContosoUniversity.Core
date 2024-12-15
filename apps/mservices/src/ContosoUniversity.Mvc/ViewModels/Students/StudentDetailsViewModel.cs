namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Application.ApiClients;

public class StudentDetailsViewModel
{
    public StudentDetailsViewModel(Student student, Dictionary<Guid, string> courseTitles)
    {
        ArgumentNullException.ThrowIfNull(student, nameof(student));
        ArgumentNullException.ThrowIfNull(courseTitles, nameof(courseTitles));

        LastName = student.LastName;
        FirstMidName = student.FirstName;
        EnrollmentDate = student.EnrollmentDate;
        ExternalId = student.ExternalId;
        Enrollments = student.Enrollments.Select(x => new EnrollmentViewModel
        {
            CourseTitle = courseTitles[x.CourseId],
            Grade = x.Grade.ToDisplayString()
        }).ToArray();
    }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; }

    public EnrollmentViewModel[] Enrollments { get; }

    [Display(Name = "Last Name")] public string LastName { get; }

    [Display(Name = "First Name")] public string FirstMidName { get; }

    public Guid ExternalId { get; }
}
