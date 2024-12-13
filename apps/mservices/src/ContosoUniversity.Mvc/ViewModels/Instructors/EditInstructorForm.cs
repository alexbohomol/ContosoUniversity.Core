namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System.ComponentModel.DataAnnotations;

using Application.ApiClients;

using global::Departments.Core.Projections;

public record EditInstructorForm(Instructor Instructor, Course[] Courses)
{
    public EditInstructorRequest Request { get; init; } = new();

    [Display(Name = "Assigned Courses")]
    public AssignedCourseOption[] AssignedCourses =>
        Courses.ToAssignedCourseOptions(Instructor.Courses);
}
