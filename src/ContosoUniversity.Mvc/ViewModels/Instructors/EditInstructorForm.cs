namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Instructors.Commands;

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

    public EditInstructorForm(Instructor instructor, Course[] courses)
    {
        ExternalId = instructor.ExternalId;
        LastName = instructor.LastName;
        FirstName = instructor.FirstName;
        HireDate = instructor.HireDate;
        Location = instructor.Office;
        AssignedCourses = courses.ToAssignedCourseOptions(instructor.Courses);
    }

    [Display(Name = "Assigned Courses")] public AssignedCourseOption[] AssignedCourses { get; }
}
