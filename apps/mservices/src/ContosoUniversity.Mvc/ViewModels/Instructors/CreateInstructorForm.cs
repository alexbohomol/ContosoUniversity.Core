namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System.ComponentModel.DataAnnotations;

using Application.ApiClients;

public record CreateInstructorForm(Course[] Courses)
{
    public CreateInstructorRequest Request { get; init; } = new();

    [Display(Name = "Assigned Courses")]
    public AssignedCourseOption[] AssignedCourses => Courses.ToAssignedCourseOptions();
}
