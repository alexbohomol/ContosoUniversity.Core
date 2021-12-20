namespace ContosoUniversity.Mvc.ViewModels.Students;

using System.ComponentModel.DataAnnotations;

public class EnrollmentViewModel
{
    [Display(Name = "Course")] public string CourseTitle { get; init; }

    public string Grade { get; init; }
}