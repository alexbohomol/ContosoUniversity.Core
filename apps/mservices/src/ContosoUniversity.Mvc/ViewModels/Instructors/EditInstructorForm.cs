namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System.ComponentModel.DataAnnotations;

using Application.ApiClients;

public record EditInstructorForm(Instructor Instructor, Course[] Courses)
{
    public EditInstructorRequest Request { get; init; } = new();

    [Display(Name = "Assigned Courses")]
    public AssignedCourseOption[] AssignedCourses =>
        Courses.ToAssignedCourseOptions(Instructor.Courses);
}
