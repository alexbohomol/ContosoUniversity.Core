namespace ContosoUniversity.ViewModels.Instructors;

using System.ComponentModel.DataAnnotations;

using Services.Instructors.Commands;

public class EditInstructorForm : EditInstructorCommand
{
    public EditInstructorForm(
        EditInstructorCommand command,
        AssignedCourseOption[] assignedCourses)
    {
        LastName = command.LastName;
        FirstName = command.FirstName;
        HireDate = command.HireDate;
        Location = command.Location;
        AssignedCourses = assignedCourses;
    }

    public EditInstructorForm()
    {
    }

    [Display(Name = "Assigned Courses")] public AssignedCourseOption[] AssignedCourses { get; set; }
}