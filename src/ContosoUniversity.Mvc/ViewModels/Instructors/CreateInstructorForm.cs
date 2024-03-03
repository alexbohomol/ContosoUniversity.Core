namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System.ComponentModel.DataAnnotations;

public class CreateInstructorForm : CreateInstructorRequest
{
    public CreateInstructorForm(CreateInstructorRequest command, AssignedCourseOption[] assignedCourses)
    {
        LastName = command.LastName;
        FirstName = command.FirstName;
        HireDate = command.HireDate;
        Location = command.Location;
        AssignedCourses = assignedCourses;
    }

    public CreateInstructorForm()
    {
    }

    [Display(Name = "Assigned Courses")] public AssignedCourseOption[] AssignedCourses { get; set; }
}
